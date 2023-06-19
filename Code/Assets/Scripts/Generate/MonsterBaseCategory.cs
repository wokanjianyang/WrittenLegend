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
        public static Monster BuildMonster(int minLevel, int userLevel)
        {
            int maxLevel = Math.Min(userLevel + 1, minLevel + 9);

            List<MonsterBase> list = MonsterBaseCategory.Instance.GetAll().Where(m => m.Value.Level >= minLevel && m.Value.Level < maxLevel).Select(m => m.Value).ToList();

            int rd = RandomHelper.RandomNumber(1, list.Count + 1);
            MonsterBase config = list[rd - 1];

            int quality = maxLevel >= 10 ? RandomHelper.RandomQuality() : 1;

            Monster enemy = new Monster(config.Id, quality);
            return enemy;
        }
    }
}