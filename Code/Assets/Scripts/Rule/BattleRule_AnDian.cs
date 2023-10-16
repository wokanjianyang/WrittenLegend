using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle_AnDian : ABattleRule
{
    private long MapTime = 0;
    private int MapId = 0;

    private int Count = 1; //最多数量
    protected override RuleType ruleType => RuleType.AnDian;

    public Battle_AnDian(Dictionary<string, object> param)
    {
        param.TryGetValue("MapTime", out object mapTime);
        param.TryGetValue("MapId", out object mapId);

        this.MapTime = (long)mapTime;
        this.MapId = (int)mapId;
    }

    public override void DoMapLogic(int roundNum)
    {
        if (roundNum % 5 != 0)
        {
            return;
        }

        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        if (enemys.Count >= 20)
        {
            return;
        }

        MapConfig mapConfig = MapConfigCategory.Instance.Get(MapId);

        if (Count % 500 != 0)
        {
            int quality = 1;
            if (Count % 100 == 0)
            {
                quality = 4;
            }
            else if (Count % 30 == 0)
            {
                quality = 3;
            }
            else if (Count % 10 == 0)
            {
                quality = 2;
            }

            var enemy = MonsterHelper.BuildMonster(mapConfig, quality, 1, 1);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        }
        else
        {
            BossConfig bossConfig = BossConfigCategory.Instance.Get(mapConfig.BoosId);
            GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(mapConfig.BoosId, mapConfig.Id, 2, 1, 1));
        }

        GameProcessor.Inst.EventCenter.Raise(new ShowAnDianInfoEvent() { Count = Count });
        Count++;
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP == 0)
        {
            hero.Resurrection();
        }
    }
}
