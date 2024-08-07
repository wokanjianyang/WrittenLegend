using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Infinite : ABattleRule
{
    private bool Start = true;

    private bool Over = true;

    //private long Progress = 1;

    private const int MaxProgress = 900; //
    private const int SkipTime = 15;
    private const int SkipCount = 10;

    private int[] MonsterList = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };
    private long AttckTime = 0;
    private long UseTime = 0;

    protected override RuleType ruleType => RuleType.Infinite;

    public BattleRule_Infinite(Dictionary<string, object> param)
    {
        //param.TryGetValue("progress", out object progress);
        param.TryGetValue("count", out object count);

        //this.Progress = (long)progress;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!this.Over)
        {
            return;
        }

        if (!Start)
        {
            //倒计时
            this.UseTime = this.AttckTime + SkipTime - TimeHelper.ClientNowSeconds();
            GameProcessor.Inst.EventCenter.Raise(new ShowInfiniteInfoEvent() { Time = UseTime });
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        User user = GameProcessor.Inst.User;
        InfiniteRecord record = user.InfiniteData.GetCurrentRecord();

        long currentProgres = record.Progress.Data;

        if (enemys.Count <= 0 && currentProgres <= MaxProgress && this.Start)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Infinite, Message = "第" + currentProgres + "波发起了进攻" });

            this.AttckTime = TimeHelper.ClientNowSeconds();

            //Load All
            for (int i = 0; i < MonsterList.Length; i++)
            {
                var enemy = new Monster_Infinite(currentProgres, MonsterList[i]);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowInfiniteInfoEvent() { Count = currentProgres, PauseCount = record.Count.Data, Time = SkipTime });

            this.Start = false;

            return;
        }

        if (enemys.Count <= 0 && !this.Start)
        {
            long progess = user.GetAchievementProgeress(AchievementSourceType.Infinite);
            long ap = 1;
            if (UseTime >= 0 && currentProgres < progess)
            {
                ap = Math.Min(progess - currentProgres, 10);
            }

            if (progess < currentProgres)
            {
                user.MagicRecord[AchievementSourceType.Infinite].Data = currentProgres;
            }

            record.Progress.Data += ap;

            for (int i = 0; i < ap; i++)
            {
                BuildReward(currentProgres + i);
            }

            this.Start = true;
            return;
        }

        if (currentProgres > MaxProgress && this.Over)
        {
            this.Over = false;
            user.InfiniteData.Complete();
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Infinite, Message = "无尽闯关成功，您就是神！！！" });
            GameProcessor.Inst.CloseBattle(RuleType.Infinite, 0);
            return;
        }
    }

    private void BuildReward(long level)
    {
        InfiniteConfig rewardConfig = InfiniteConfigCategory.Instance.GetByLevel(level);

        User user = GameProcessor.Inst.User;

        long exp = (long)rewardConfig.Exp;
        long gold = (long)rewardConfig.Gold;

        //增加经验,金币
        user.AddExpAndGold(exp, gold);

        List<KeyValuePair<double, DropConfig>> dropList = new List<KeyValuePair<double, DropConfig>>();

        //掉落道具
        int dropId = user.InfiniteData.GetDropId((int)level);
        DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

        dropList.Add(new KeyValuePair<double, DropConfig>(1, dropConfig));

        List<Item> items = DropHelper.BuildDropItem(dropList, 1);

        if (items.Count > 0)
        {
            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Infinite,
            Message = BattleMsgHelper.BuildRewardMessage("无尽闯关" + level + "奖励:", exp, gold, items)
        });
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero != null && hero.HP == 0)
        {
            User user = GameProcessor.Inst.User;
            InfiniteRecord record = user.InfiniteData.GetCurrentRecord();

            if (record == null)
            {
                return;
            }

            record.Count.Data--;
            GameProcessor.Inst.EventCenter.Raise(new ShowInfiniteInfoEvent() { Count = record.Progress.Data, PauseCount = record.Count.Data });

            if (record.Count.Data > 0)
            {
                GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
                GameProcessor.Inst.HeroDie(RuleType.Infinite, 0);
            }
            else
            {
                this.Over = false;
                user.InfiniteData.Complete();
                GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Infinite, Message = "无尽闯关失败，请明天再来" });
                GameProcessor.Inst.CloseBattle(RuleType.Infinite, 0);
            }
        }
    }
}
