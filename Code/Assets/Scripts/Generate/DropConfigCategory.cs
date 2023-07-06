using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class DropConfigCategory
    {
        public List<KeyValuePair<int, DropConfig>> GetByMapLevel(int mapLevel, int rate)
        {
            List<KeyValuePair<int, DropConfig>> list = new List<KeyValuePair<int, DropConfig>>();

            MapConfig map = MapConfigCategory.Instance.GetAll().Where(m => m.Value.MonsterLevelMin <= mapLevel && m.Value.MonsterLevelMax > mapLevel).First().Value;

            if (map != null)
            {
                for (int i = 0; i < map.DropIdList.Length; i++)
                {
                    DropConfig dropConfig = this.Get(map.DropIdList[i]);
                    list.Add(new KeyValuePair<int, DropConfig>(map.DropRateList[i] / rate, dropConfig));
                }
            }

            return list;
        }
    }

    public class DropHelper
    {
        public static List<Item> BuildDropItem(List<KeyValuePair<int, DropConfig>> dropList, int minQuanlity)
        {
            List<Item> list = new List<Item>();

            for (int i = 0; i < dropList.Count; i++)
            {
                int rate = dropList[i].Key;
                DropConfig config = dropList[i].Value;

                if (RandomHelper.RandomResult(rate))
                {
                    int index = RandomHelper.RandomNumber(0, config.ItemIdList.Length);
                    int configId = config.ItemIdList[index];

                    Item item = ItemHelper.BuildItem((ItemType)config.ItemType, configId, minQuanlity, 1);
                    list.Add(item);
                }
            }
            return list;
        }
    }
}