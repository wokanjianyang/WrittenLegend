using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRule_Tower : ABattleRule
{
    public override void DoHeroLogic()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        hero.DoEvent();
    }

    public override void DoMonsterLogic()
    {
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        enemys.Sort((a, b) =>
        {
            var distance = a.Cell - hero.Cell;
            var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);

            distance = b.Cell - hero.Cell;
            var l1 = Math.Abs(distance.x) + Math.Abs(distance.y);

            if (l0 < l1)
            {
                return -1;
            }
            else if (l0 > l1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        });

        foreach (var enemy in enemys)
        {
            enemy.DoEvent();
        }

        if (enemys.Count == 0) //TODO ���Լ���ˢ������
        {
            var enemy = MonsterTowerHelper.BuildMonster(hero.TowerFloor - 1);
            if (enemy != null)
            {
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }
        }
    }

    public override void DoValetLogic()
    {
    }

    protected override bool CheckGameResult()
    {
        var result = false;
        var hero = GameProcessor.Inst.PlayerManager.GetHero();
        if (hero.HP == 0)
        {
            this.winCamp = PlayerType.Enemy;
            result = true;
        }

        return result && this.roundNum > 1;
    }
}
