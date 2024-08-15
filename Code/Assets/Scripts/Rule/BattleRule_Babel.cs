using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Babel : ABattleRule
{
    private bool Start = true;

    private bool Over = true;

    private long Progress = 0;

    private int TimeTotal = 120;

    private int[] MonsterList = new int[] { 5, 4, 4, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1 };
    private long AttckTime = 0;
    private long UseTime = 0;

    protected override RuleType ruleType => RuleType.Infinite;

    public BattleRule_Babel(Dictionary<string, object> param)
    {
        //param.TryGetValue("progress", out object progress);
        param.TryGetValue("progress", out object progress);

        this.Progress = (long)progress;
        TimeTotal = 120;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (!Start)
        {
            var RealBoss = new Monster_Babel(Progress);  //刷新本体,10代表满血
            GameProcessor.Inst.PlayerManager.LoadMonster(RealBoss);
            Start = true;
        }

        TimeTotal--;

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Babel, Message = "英雄死亡,挑战失败！" });
            Start = false;
        }

        if (TimeTotal <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Babel, Message = "时间超时,挑战失败！" });
            Start = false;
            GameProcessor.Inst.HeroDie(RuleType.Babel, 0);
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count <= 0)
        {
            BuildReward(this.Progress);

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.Babel, Message = "第" + Progress + "关挑战成功！" });
            GameProcessor.Inst.CloseBattle(RuleType.Babel, 0);

            return;
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
            Type = RuleType.Infinite,
            Message = BattleMsgHelper.BuildRewardMessage("通天塔奖励:" + progress + "奖励:", 0, 0, items)
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
