using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class TowerConfigCategory
    {

    }

    public class MonsterTowerHelper
    {
        public static List<Monster_Tower> BuildMonster(int floor)
        {
            var config = TowerConfigCategory.Instance.Get(floor);
            if (config != null)
            {
                List<Monster_Tower> monsters = new List<Monster_Tower>();
                for (int i = 0; i < config.Quantity; i++)
                {
                    var enemy = new Monster_Tower(floor, i);
                    monsters.Add(enemy);
                }

                return monsters;
            }
            return null;
        }
    }
}
