using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle_AnDian : ABattleRule
{
    private int MapId = 0;

    private int Level = 1;
    private int Count = 1;
    protected override RuleType ruleType => RuleType.AnDian;

    public Battle_AnDian(Dictionary<string, object> param)
    {
        param.TryGetValue("MapId", out object mapId);

        this.MapId = (int)mapId;


        GameProcessor.Inst.EventCenter.AddListener<AnDianChangeLevel>(this.OnAnDianChangeLevel);
    }

    public void OnAnDianChangeLevel(AnDianChangeLevel e)
    {
        this.Level = e.Level;
    }

    public override void DoMapLogic(int roundNum)
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

        int tempMapId = Math.Max(MapId - 10 + Level, ConfigHelper.MapStartId);

        MapConfig mapConfig = MapConfigCategory.Instance.Get(tempMapId);

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

            var enemy = MonsterBaseCategory.Instance.BuildMonster(mapConfig, quality, 1, 1,RuleType.AnDian);
            GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        }
        else
        {
            BossConfig bossConfig = BossConfigCategory.Instance.Get(mapConfig.BoosId);
            GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(mapConfig.BoosId, mapConfig.Id, RuleType.AnDian, 1, 1));
        }

        //Specail
        List<MonsterSpecialConfig> configs = MonsterSpecialConfigCategory.Instance.GetAll().Values.Where(m => m.BuildRate < Count).ToList();
        foreach (MonsterSpecialConfig config in configs)
        {
            if (RandomHelper.RandomNumber(1, config.BuildRate) <= 1)
            {
                GameProcessor.Inst.PlayerManager.LoadMonster(new Monster_Specail(config.Id, 1, RuleType.AnDian));
            }
        }

        GameProcessor.Inst.EventCenter.Raise(new ShowAnDianInfoEvent() { Count = Count });
        Count++;
    }

    public override void CheckGameResult()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP == 0)
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Enemy);
            GameProcessor.Inst.HeroDie(RuleType.AnDian, 0);
        }
    }
}
