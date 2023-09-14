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
        private bool start = false;

        protected override RuleType ruleType => RuleType.Normal;

        public override void DoMapLogic()
        {
            var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);

            if (start)
            {
                if (enemys.Count == 0)
                {
                    start = false;
                    MakeReward();

                    Hero hero = GameProcessor.Inst.PlayerManager.GetHero();
                    hero.Resurrection();

                    GameProcessor.Inst.EventCenter.Raise(new ChangeFloorEvent() { });
                }
            }
            else
            {
                NewFloor();
            }
        }

        private void NewFloor() {
            User user = GameProcessor.Inst.User;
            var monsters = MonsterTowerHelper.BuildMonster(user.MagicTowerFloor.Data);
            if (monsters != null && monsters.Count > 0)
            {
                monsters.ForEach(enemy =>
                {
                    GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
                });
                start = true;
            }
        }

        protected void MakeReward()
        {
            //Log.Info("Tower Success");
            User user = GameProcessor.Inst.User;

            TowerConfig config = TowerConfigCategory.Instance.GetByFloor(user.MagicTowerFloor.Data);

            MonsterTowerHelper.GetTowerSecond(user.MagicTowerFloor.Data, out long secondExp, out long secondGold);

            user.AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, secondExp);
            user.AttributeBonus.SetAttr(AttributeEnum.SecondGold, AttributeFrom.Tower, secondGold);

            int equipLevel = Math.Max(10, (user.MapId - ConfigHelper.MapStartId) * 10);

            List<Item> items = DropHelper.TowerEquip(user.MagicTowerFloor.Data, equipLevel);

            if (items.Count > 0)
            {
                user.EventCenter.Raise(new HeroBagUpdateEvent() { ItemList = items });
            }

            long exp = config.Exp;
            long gold = config.Gold;
            user.AddExpAndGold(exp, gold);

            GameProcessor.Inst.EventCenter.Raise(new BattleMsgEvent()
            {
                Message = BattleMsgHelper.BuildTowerSuccessMessage(config.RiseExp, config.RiseGold, exp, gold, user.MagicTowerFloor.Data, items),
                BattleType = BattleType.Tower
            });

            user.MagicTowerFloor.Data++;

            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });
            }

            //判断任务
            TaskHelper.CheckTask(TaskType.Tower, user.MagicTowerFloor.Data);
        }

        //public override void DoHeroLogic()
        //{
        //    var hero = GameProcessor.Inst.PlayerManager.GetHero();
        //    if (hero != null)
        //    {
        //        //hero.DoEvent();
        //    }
        //}

        //public override void DoMonsterLogic()
        //{
        //    User user = GameProcessor.Inst.User;

        //    Hero hero = GameProcessor.Inst.PlayerManager.GetHero();
        //    var enemys = GameProcessor.Inst.PlayerManager.GetPlayersByCamp(PlayerType.Enemy);
        //    enemys.Sort((a, b) =>
        //    {
        //        var distance = a.Cell - hero.Cell;
        //        var l0 = Math.Abs(distance.x) + Math.Abs(distance.y);

        //        distance = b.Cell - hero.Cell;
        //        var l1 = Math.Abs(distance.x) + Math.Abs(distance.y);

        //        if (l0 < l1)
        //        {
        //            return -1;
        //        }
        //        else if (l0 > l1)
        //        {
        //            return 1;
        //        }
        //        else
        //        {
        //            return 0;
        //        }
        //    });

        //    foreach (var enemy in enemys)
        //    {
        //        //enemy.DoEvent();
        //    }

        //    if (enemys.Count == 0) //TODO 闯关条件
        //    {
        //        if (start == true)
        //        { //闯关奖励
        //            MakeReward();

        //            // if (GameProcessor.Inst.RefreshSkill)
        //            // {
        //            //     GameProcessor.Inst.RefreshSkill = false;
        //            //     hero.OnDestroy();
        //            //     GameProcessor.Inst.PlayerManager.LoadHero();
        //            // }
        //            // else
        //            {
        //                //刷新英雄属性
        //                hero.EventCenter.Raise(new HeroLevelUp());
        //            }

        //            //自动跳转
        //            GameProcessor.Inst.EventCenter.Raise(new ChangeFloorEvent() { });
        //        }

        //        var monsters = MonsterTowerHelper.BuildMonster(user.MagicTowerFloor.Data);
        //        if (monsters != null && monsters.Count > 0)
        //        {
        //            start = true;
        //            monsters.ForEach(enemy =>
        //            {
        //                GameProcessor.Inst.PlayerManager.LoadMonster(enemy);
        //            });
        //        }
        //    }
        //}

    }
}
