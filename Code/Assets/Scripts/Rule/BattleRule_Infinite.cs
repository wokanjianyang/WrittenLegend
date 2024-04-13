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

    private int Level = 0;
    private long Progress = 1;

    private const int MaxProgress = 1000; //

    private long PauseCount = 0;

    private int[] MonsterList = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };

    protected override RuleType ruleType => RuleType.Infinite;

    public BattleRule_Infinite(Dictionary<string, object> param)
    {
        param.TryGetValue("progress", out object progress);
        param.TryGetValue("count", out object count);

        this.Progress = (long)progress;
        this.PauseCount = (long)count;
    }

    public override void DoMapLogic(int roundNum)
    {
        if (!this.Over)
        {
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count > 0)
        {
            return;
        }

        if (enemys.Count <= 0 && this.Progress <= MaxProgress && this.Start)
        {
            this.Progress++;

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "第" + this.Progress + "波发起了进攻" });

            //Load All
            for (int i = 0; i < MonsterList.Length; i++)
            {
                var enemy = new Monster_Infinite(this.Progress, MonsterList[i]);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowInfiniteInfoEvent() { Count = Progress, PauseCount = PauseCount });

            this.Start = false;

            return;
        }

        User user = GameProcessor.Inst.User;

        if (enemys.Count <= 0 && !this.Start)
        {
            //check 
            long progess = user.GetAchievementProgeress(AchievementSourceType.Defend);
            long cp = (this.Level - 1) * 100 + this.Progress;
            if (progess < cp)
            {
                user.MagicRecord[AchievementSourceType.Defend].Data = cp;
            }

            this.Start = true;

            BuildReward(this.Progress);

            this.Progress++;

            DefendRecord record = user.DefendData.GetCurrentRecord();

            record.Progress.Data = this.Progress;


            return;
        }

        if (this.Progress > MaxProgress && this.Over)
        {
            this.Over = false;

            user.DefendData.Complete();


            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "守卫成功" });

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);

            return;
        }
    }

    private void BuildReward(long level)
    {
        InfiniteConfig rewardConfig = InfiniteConfigCategory.Instance.GetByLevel(level);

        User user = GameProcessor.Inst.User;

        long exp = (long)(rewardConfig.Exp * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.ExpIncrea)) / 100);
        long gold = (long)(rewardConfig.Gold * (100 + user.AttributeBonus.GetTotalAttr(AttributeEnum.GoldIncrea)) / 100);

        //增加经验,金币
        user.AddExpAndGold(exp, gold);

        List<KeyValuePair<double, DropConfig>> dropList = new List<KeyValuePair<double, DropConfig>>();

        //掉落道具
        List<InfiniteDropConfig> infinites = InfiniteDropConfigCategory.Instance.GetLevelList(level);
        foreach (InfiniteDropConfig config in infinites)
        {
            DropConfig dropConfig = DropConfigCategory.Instance.Get(config.DropId);
            dropList.Add(new KeyValuePair<double, DropConfig>(config.Rate, dropConfig));
        }

        List<Item> items = DropHelper.BuildDropItem(dropList, 1);

        if (items.Count > 0)
        {
            user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Infinite,
            Message = BattleMsgHelper.BuildRewardMessage("无尽闯关奖励:", exp, gold, items)
        });
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP == 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Defend, 0);
        }
    }
}
