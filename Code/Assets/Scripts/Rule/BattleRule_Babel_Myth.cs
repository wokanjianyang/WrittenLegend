using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Babel_Myth : ABattleRule
{
    private bool Start = false;
    private bool Over = false;

    private long Progress = 0;

    private const double TimeMax = 180;
    private double TimeTotal = 0;

    private int[] MonsterList1 = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    private int[] MonsterList2 = new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 3 };
    private int[] MonsterList3 = new int[] { 2, 2, 2, 2, 2, 2, 3, 3, 3, 3 };

    protected override RuleType ruleType => RuleType.BabelMyth;

    public BattleRule_Babel_Myth(Dictionary<string, object> param)
    {
        User user = GameProcessor.Inst.User;

        this.Progress = user.BabelMythData.Data + 1;
        TimeTotal = TimeMax;

        user.BabelMythCount.Data--;

        this.LoadHero();
    }

    private void LoadHero()
    {
        HeroMyth hero = new HeroMyth(true);
        GameProcessor.Inst.PlayerManager.LoadHero(hero);
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (Over || this.Progress > ConfigHelper.BabelMythMax)
        {
            return;
        }

        if (!Start)
        {
            Start = true;

            int[] types = CalTypes(Progress);

            foreach (int type in types)
            {
                var monster = new Monster_Babel_Myth(Progress, type);
                GameProcessor.Inst.PlayerManager.LoadMonster(monster);
            }

            return;
        }

        User user = GameProcessor.Inst.User;
        TimeTotal -= currentRoundTime;
        GameProcessor.Inst.EventCenter.Raise(new ShowBabelInfoEvent() { Progress = this.Progress, Time = TimeTotal, Count = user.BabelMythCount.Data });

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.IsDie() || TimeTotal <= 0 || user.BabelMythCount.Data <= 0)
        {
            Over = true;

            user.BabelMythCount.Data--;
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.BabelMyth, Message = "挑战失败！" });
            GameProcessor.Inst.HeroDie(RuleType.Babel, 0);
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (!Over && enemys.Count <= 0)
        {
            //Over = true;

            user.BabelMythData.Data++;
            user.BabelMythCount.Data--;
            BuildReward(this.Progress);

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.BabelMyth, Message = "第" + Progress + "关挑战成功！" });

            if (user.BabelMythCount.Data <= 0)
            {
                GameProcessor.Inst.CloseBattle(RuleType.BabelMyth, 0);
            }
            else
            {
                TimeTotal = TimeMax;
                Start = false;
            }

            this.Progress = GameProcessor.Inst.User.BabelMythData.Data + 1;

            return;
        }
    }

    private int[] CalTypes(long progress)
    {
        if (progress % 20 == 0)
        {
            return MonsterList3;
        }
        else if (progress % 5 == 0)
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
        BabelMythConfig rewardConfig = BabelMythConfigCategory.Instance.GetByProgress(progress);

        //掉落道具
        List<Item> items = new List<Item>();
        items.Add(rewardConfig.BuildItem(progress));

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.BabelMyth,
            Message = BattleMsgHelper.BuildRewardMessage("通天塔奖励:" + progress + "奖励:", 0, 0, items)
        });

        GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });

        //if (AppHelper.BabelMythRecord > 0 && progress >= AppHelper.BabelMythRecord + 10)
        //{
        //    AppHelper.BabelMythRecord = (int)progress;
        //    GameProcessor.Inst.SaveRecord("babel_myth", progress + "");
        //}
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
