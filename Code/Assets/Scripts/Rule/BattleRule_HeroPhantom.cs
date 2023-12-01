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

    protected override RuleType ruleType => RuleType.HeroPhantom;

    private APlayer PhantomPlayer = null;

    public BattleRule_HeroPhantom(Dictionary<string, object> param)
    {
        Start = true;

        Level = (int)GameProcessor.Inst.User.HeroPhatomData.Current.Progress.Data;

        this.LoadPhantom();
    }

    private void LoadPhantom()
    {
        GameProcessor.Inst.PlayerManager.LoadHeroPvp();

        this.PhantomPlayer = new HeroPhantom();
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
            GameProcessor.Inst.EventCenter.Raise(new BattlePhantomMsgEvent() { Message = "你没有通过挑战！" });
            Start = false;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

        if (enemys.Count <= 0)
        {
            GameProcessor.Inst.User.HeroPhatomData.Current.Next();
            //reward


            GameProcessor.Inst.EventCenter.Raise(new BattlePhantomMsgEvent() { Message = "您已经通过了挑战！" });

            GameOver();

            Start = false;
            return;
        }
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
        GameProcessor.Inst.HeroDie(RuleType.HeroPhantom, 0);
    }
}
