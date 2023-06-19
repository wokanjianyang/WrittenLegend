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

            User user = GameProcessor.Inst.User;
            if (user.MapId == 0)
            {
                user.MapId = MapConfigCategory.Instance.GetAll().First().Value.Id;
            }

            MapConfig mapConfig = MapConfigCategory.Instance.Get(user.MapId); ;

            if (enemys.Count <= 20) //TODO 
            {
                var enemy = MonsterHelper.BuildMonster(mapConfig.LevelRequired,user.Level);
                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
            }

            var boss = GameProcessor.Inst.PlayerManager.GetBoss();
            if (boss == null && user.Level >= 5)
            {
                BossConfig bossConfig = BossConfigCategory.Instance.Get(mapConfig.BoosId);
                long killTime = 1;
                if (user.MapBossTime.TryGetValue(user.MapId, out killTime))
                {
                    long currentTime = TimeHelper.ClientNowSeconds();
                    if (killTime == 0 || currentTime - killTime >= mapConfig.BossInterval * 60)
                    {
                        GameProcessor.Inst.PlayerManager.LoadMonster(BossHelper.BuildBoss(mapConfig.BoosId, mapConfig.Id));
                    }
                }
            }
        }
    }
}
