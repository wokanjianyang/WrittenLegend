using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_HeroPhantom : ABattleRule
{
    private bool Start = false;

    private int Level = 0;
    private long MapTime = 0;
    protected override RuleType ruleType => RuleType.HeroPhantom;

    private APlayer PhantomPlayer = null;

    public BattleRule_HeroPhantom(Dictionary<string, object> param)
    {
        param.TryGetValue("MapTime", out object mapTime);

        this.MapTime = (long)mapTime;
        Start = true;

        Level = (int)GameProcessor.Inst.User.HeroPhatomData.Current.Progress.Data;

        this.LoadPhantom();
    }

    private void LoadPhantom()
    {
        GameProcessor.Inst.PlayerManager.LoadHeroPvp(RuleType.HeroPhantom);

        this.PhantomPlayer = new HeroPhantom(Level);
        GameProcessor.Inst.PlayerManager.LoadHeroPhantom(this.PhantomPlayer);

        this.Start = true;
    }


    public override void DoMapLogic(int roundNum)
    {
        if (!Start)
        {
            return;
        }

        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP <= 0)
        {
            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.HeroPhantom, Message = "你没有通过挑战！" });
            Start = false;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.HeroPhatom);

        if (enemys.Count <= 0)
        {
            GameProcessor.Inst.User.HeroPhatomData.Current.Next();
            //reward

            BuildReward();

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent() { Type = RuleType.HeroPhantom, Message = "您已经通过了挑战！" });

            GameOver();

            Start = false;
            return;
        }
    }

    private void BuildReward()
    {
        User user = GameProcessor.Inst.User;

        List<Item> items = DropLimitHelper.Build((int)DropLimitType.HeroPhatom, 0, 1, 1, 1, 1);

        if (items.Count > 0)
        {
            GameProcessor.Inst.User.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
        }

        GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
        {
            Type = RuleType.HeroPhantom,
            Message = BattleMsgHelper.BuildRewardMessage("挑战成功奖励", 0, 0, items)
        });
    }

    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
        GameProcessor.Inst.HeroDie(RuleType.HeroPhantom, MapTime);
    }
}
