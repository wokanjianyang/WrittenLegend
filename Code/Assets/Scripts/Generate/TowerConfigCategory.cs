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
        public static Monster_Tower BuildMonster(int floor)
        {
            var config = TowerConfigCategory.Instance.Get(floor);
            if(config!=null)
            {
                var enemy = new Monster_Tower();
                enemy.Load();
                enemy.SetLevelConfigAttr(config);
                enemy.Logic.SetData(null);

                return enemy;
            }

            return null;
        }
    }
}
