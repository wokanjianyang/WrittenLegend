using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class BattleRule_Normal : ABattleRule
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

            if (enemys.Count <= 20) //TODO 测试减少刷新数量
            {
                var enemy = MonsterHelper.BuildMonster(hero.Level);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }
        }
    }
}
