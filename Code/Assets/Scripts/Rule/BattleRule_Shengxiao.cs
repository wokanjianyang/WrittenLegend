using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Shengxiao : ABattleRule
{
    private int MapId = 0;

    private double TotalTime = 0;

    private int Count = 0;

    protected override RuleType ruleType => RuleType.Shengxiao;

    public BattleRule_Shengxiao(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);

        this.MapId = (int)mapId;
        this.LoadHero();
    }

    private void LoadHero()
    {
        HeroMyth hero = new HeroMyth(false);
        GameProcessor.Inst.PlayerManager.LoadHero(hero);

        TotalTime = 0;
        Count = 0;
    }



    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        TotalTime += currentRoundTime;
        GameProcessor.Inst.EventCenter.Raise(new ShowShengxiaoInfoEvent() { Count = Count, Time = (int)TotalTime });

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        if (enemys.Count >= 30)
        {
            return;
        }

        int quality = BuildQuality();

        var enemy = new Monster_Shengxiao(MapId, quality);
        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);

        Count++;
    }

    private int BuildQuality()
    {
        int rd = RandomHelper.RandomNumber(1, 501);
        if (rd > 499)
        {
            return 5;
        }
        else if (rd > 495)
        {
            return 4;
        }
        else if (rd > 480)
        {
            return 3;
        }
        else if (rd > 400)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }


    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP <= 0)
        {
            AppHelper.Shengxiao_Id = this.MapId;

            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Shengxiao, 18);

        }
    }
}
