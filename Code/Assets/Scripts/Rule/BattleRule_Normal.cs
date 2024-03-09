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

        public override void DoMapLogic(int roundNum)
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
                if (roundNum % 4 != 0)
                {
                    return;
                }

                NewFloor();
            }
        }

        private void NewFloor()
        {
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

            if (user.MagicTowerFloor.Data >= ConfigHelper.Max_Floor)
            {
                return;
            }

            TowerConfig config = TowerConfigCategory.Instance.GetByFloor(user.MagicTowerFloor.Data);

            int floorRate = ConfigHelper.GetFloorRate(user.MagicTowerFloor.Data) * user.GetDzRate();

            MonsterTowerHelper.GetTowerSecond(user.MagicTowerFloor.Data, out long secondExp, out long secondGold);

            user.AttributeBonus.SetAttr(AttributeEnum.SecondExp, AttributeFrom.Tower, secondExp);
            user.AttributeBonus.SetAttr(AttributeEnum.SecondGold, AttributeFrom.Tower, secondGold);

            int equipLevel = Math.Max(10, (user.MapId - ConfigHelper.MapStartId) * 10);

            List<Item> items = new List<Item>();

            for (int i = 0; i < floorRate; i++)
            {
                items.AddRange(DropHelper.TowerEquip(user.MagicTowerFloor.Data + i, equipLevel));
            }

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
                Type = RuleType.Normal
            });

            user.MagicTowerFloor.Data += floorRate;

            //自动回收
            if (items.Count > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { RuleType = RuleType.Normal });
            }

            //判断任务
            TaskHelper.CheckTask(TaskType.Tower, user.MagicTowerFloor.Data);
        }

    }
}
