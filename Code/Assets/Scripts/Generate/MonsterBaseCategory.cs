using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBaseCategory
    {

    }

    public class MonsterHelper
    {
        public static Monster BuildMonster(int level)
        {
            List<MonsterBase> list = MonsterBaseCategory.Instance.GetAll().Where(m => m.Value.Level >= level && m.Value.Level < level + 10).Select(m=>m.Value).ToList();

            int rd = RandomHelper.RandomNumber(1, list.Count+1);
            MonsterBase config = list[rd - 1];

            Monster enemy = new Monster();
            enemy.Load();
            enemy.SetLevelConfigAttr(config);
            enemy.Logic.SetData(null);

            return enemy;
        }
    }
}