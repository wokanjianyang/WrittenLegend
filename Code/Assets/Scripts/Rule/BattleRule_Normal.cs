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
            var heros = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Hero);
            if(heros == null || heros.Count == 0)
            {
                GameProcessor.Inst.PlayerManager.LoadHero();
            }
            else
            {
                foreach (var hero in heros)
                {
                    hero.DoEvent();
                }
            }
        }

        public override void DoValetLogic()
        {
            var heros = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Hero);
            if (heros != null && heros.Count > 0)
            {
                var hero = heros[0];
                var valets = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Valet);

                foreach (var valet in valets)
                {
                    valet.DoEvent();
                }
            }
        }

        public override void DoMonsterLogic()
        {
            var heros = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Hero);
            if (heros != null && heros.Count > 0)
            {
                var hero = heros[0];
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

                if (enemys.Count <= 30) //TODO
                {
                    GameProcessor.Inst.PlayerManager.LoadMonster();
                }
            }
        }
    }
}
