using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle_Defend : ABattleRule
{
    private bool Start = true;

    private bool Over = true;

    private int Level = 0;
    private long Progress = 0;

    private const int MaxProgress = 100; //

    private long PauseCount = 0;

    private int[] MonsterList = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };

    protected override RuleType ruleType => RuleType.Defend;

    public Battle_Defend(Dictionary<string, object> param)
    {
        param.TryGetValue("progress", out object progress);
        param.TryGetValue("hp", out object hp);
        param.TryGetValue("count", out object count);

        this.Progress = (long)progress;
        this.PauseCount = (long)count;
        this.Level = AppHelper.DefendLevel;

        this.LoadDefend((long)hp);
    }

    private Defend defendPlayer = null;

    private void LoadDefend(long hp)
    {
        this.defendPlayer = new Defend(hp);
        GameProcessor.Inst.PlayerManager.LoadDefend(this.defendPlayer);

        this.Start = true;
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
            int si = (int)(this.Progress - 1) / 10 + 1;

            if (!GameProcessor.Inst.User.DefendData.GetCurrentRecord().BuffDict.ContainsKey(si))
            {
                GameProcessor.Inst.EventCenter.Raise(new DefendBuffSelectEvent() { Index = si, Level = this.Level });
            }

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "第" + this.Progress + "波发起了进攻" });

            //Load All
            for (int i = 0; i < MonsterList.Length; i++)
            {
                var enemy = new Monster_Defend(this.Progress, MonsterList[i], this.Level);
                GameProcessor.Inst.PlayerManager.LoadMonsterDefend(enemy);
            }

            GameProcessor.Inst.EventCenter.Raise(new ShowDefendInfoEvent() { Count = Progress, PauseCount = PauseCount });

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
            this.Progress++;

            DefendRecord record = user.DefendData.GetCurrentRecord();

            record.Progress.Data = this.Progress;
            record.Hp.Data = (long)defendPlayer.HP;

            return;
        }



        if (this.Progress > MaxProgress && this.Over)
        {
            this.Over = false;

            user.DefendData.Complete();

            BuildReward();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "守卫成功" });

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);

            return;
        }
    }

    private void BuildReward()
    {
        User user = GameProcessor.Inst.User;

        List<Item> items = DropLimitHelper.Build((int)DropLimitType.Defend, 0, 1, 1, 1,1);

        if (items.Count > 0)
        {
            GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Defend,
            Message = BattleMsgHelper.BuildRewardMessage("守卫成功奖励", 0, 0, items)
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

        var defend = GameProcessor.Inst.PlayerManager.GetDefend();
        if (defend.HP <= 0)
        {
            this.Over = false;
            GameProcessor.Inst.User.DefendData.Complete();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Defend, Message = "守卫失败" });

            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);

            GameProcessor.Inst.CloseBattle(RuleType.Defend, 0);
        }
    }
}
