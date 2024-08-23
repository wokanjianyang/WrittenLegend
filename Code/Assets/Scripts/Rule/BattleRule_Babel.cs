using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Babel : ABattleRule
{
    private bool Start = false;
    private bool Over = false;

    private long Progress = 0;

    private const double TimeMax = 180;
    private double TimeTotal = 0;

    private int[] MonsterList1 = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 2, 2 };
    private int[] MonsterList2 = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 3 };
    private int[] MonsterList3 = new int[] { 2, 2, 2, 2, 2, 2, 3, 3, 3, 3 };

    protected override RuleType ruleType => RuleType.Babel;

    public BattleRule_Babel(Dictionary<string, object> param)
    {
        //param.TryGetValue("progress", out object progress);
        User user = GameProcessor.Inst.User;

        this.Progress = user.BabelData.Data + 1;
        TimeTotal = TimeMax;

        user.BabelCount.Data--;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (Over)
        {
            return;
        }

        if (!Start)
        {
            Start = true;
            int[] types = CalTypes(Progress);

            foreach (int type in types)
            {
                var RealBoss = new Monster_Babel(Progress, type);  //刷新本体,10代表满血
                GameProcessor.Inst.PlayerManager.LoadMonster(RealBoss);
            }

            return;
        }

        User user = GameProcessor.Inst.User;
        TimeTotal -= currentRoundTime;
        GameProcessor.Inst.EventCenter.Raise(new ShowBabelInfoEvent() { Progress = this.Progress, Time = TimeTotal, Count = user.BabelCount.Data });

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP <= 0 || TimeTotal <= 0)
        {
            Over = true;

            user.BabelCount.Data--;
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Babel, Message = "挑战失败！" });
            GameProcessor.Inst.HeroDie(RuleType.Babel, 0);
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (!Over && enemys.Count <= 0)
        {
            //Over = true;

            user.BabelData.Data++;
            user.BabelCount.Data--;
            BuildReward(this.Progress);

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Babel, Message = "第" + Progress + "关挑战成功！" });

            if (user.BabelCount.Data <= 0)
            {
                GameProcessor.Inst.CloseBattle(RuleType.Babel, 0);
            }
            else
            {
                TimeTotal = TimeMax;
                Start = false;
            }

            this.Progress = GameProcessor.Inst.User.BabelData.Data + 1;

            return;
        }
    }

    private int[] CalTypes(long progress)
    {
        if (progress % 100 == 0)
        {
            return MonsterList3;
        }
        else if (progress % 10 == 0)
        {

            return MonsterList2;
        }
        else
        {
            return MonsterList1;
        }
    }

    private void BuildReward(long progress)
    {
        BabelConfig rewardConfig = BabelConfigCategory.Instance.GetByProgress(progress);

        //掉落道具
        List<Item> items = new List<Item>();
        items.Add(rewardConfig.BuildItem(progress));

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.Babel,
            Message = BattleMsgHelper.BuildRewardMessage("通天塔奖励:" + progress + "奖励:", 0, 0, items)
        });

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
    }

    public override void CheckGameResult()
    {
        //var hero = GameProcessor.Inst.PlayerManager.GetHero();
        //if (hero != null && hero.HP == 0)
        //{
        //    GameProcessor.Inst.HeroDie(RuleType.Babel, 0);
        //}
    }
}
