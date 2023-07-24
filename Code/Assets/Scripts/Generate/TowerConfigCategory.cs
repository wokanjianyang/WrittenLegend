using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class TowerConfigCategory
    {
        public TowerConfig GetByFloor(long floor)
        {
            TowerConfig config = TowerConfigCategory.Instance.GetAll().Where(m => m.Value.StartLevel <= floor && m.Value.EndLevel >= floor).First().Value;
            return config;
        }
    }

    public class MonsterTowerHelper
    {
        public static List<Monster_Tower> BuildMonster(int floor)
        {
            List<Monster_Tower> monsters = new List<Monster_Tower>();

            int monsterQuantity = 1;
            if (floor > 10000)
            {
                monsterQuantity = (floor % 10) + 1;
            }

            for (int i = 0; i < monsterQuantity; i++)
            {
                var enemy = new Monster_Tower(floor, i);
                monsters.Add(enemy);
            }

            return monsters;
        }

        public static void GetTowerSecond(long floor, out long secondExp, out long secondGold)
        {
            TowerConfig config = TowerConfigCategory.Instance.GetByFloor(floor);
            long rise = floor - config.StartLevel;

            secondExp = config.StartExp + (long)(rise * config.RiseExp);
            secondGold = config.StartGold + (long)(rise * config.RiseGold);
        }
    }
}
