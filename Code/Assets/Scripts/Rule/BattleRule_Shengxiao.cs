using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleRule_Shengxiao : ABattleRule
{
    private int MapId = 0;

    private double MapTime = 0;

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
        HeroMyth hero = new HeroMyth();
        GameProcessor.Inst.PlayerManager.LoadHero(hero);

        MapTime = 0;
    }

    public override void DoMapLogic(int roundNum, double currentRoundTime)
    {
        if (roundNum % 2 != 0)
        {
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        if (enemys.Count >= 20)
        {
            return;
        }

        int quality = RandomHelper.RandomNumber(1, 6);

        var enemy = new Monster_Shengxiao(MapId, quality);
        GameProcessor.Inst.PlayerManager.LoadMonster(enemy);

        Count++;
        GameProcessor.Inst.EventCenter.Raise(new ShowShengxiaoInfoEvent() { Count = Count });

    }


    public override void CheckGameResult()
    {
        var heroCamp = GameProcessor.Inst.PlayerManager.GetHero();
        if (heroCamp.HP <= 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.Shengxiao, 0);
        }
    }
}
