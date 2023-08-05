using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class DropConfigCategory
    {
        public List<KeyValuePair<int, DropConfig>> GetByMapLevel(int mapId, int rate)
        {
            List<KeyValuePair<int, DropConfig>> list = new List<KeyValuePair<int, DropConfig>>();

            MapConfig map = MapConfigCategory.Instance.Get(mapId);

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
        public static List<Item> TowerEquip(int equipLevel)
        {
            List<Item> list = new List<Item>();

            List<DropConfig> drops = DropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Level == equipLevel && m.ItemType == 2).ToList(); //获取当前等级的装备
            if (drops.Count > 0)
            {
                int[] ids = drops[0].ItemIdList;
                if (ids != null && ids.Length > 0)
                {
                    int index = RandomHelper.RandomNumber(0, ids.Length);

                    Item item = ItemHelper.BuildItem(ItemType.Equip, ids[index], 1, 1);
                    list.Add(item);
                }
            }
            return list;
        }

        public static List<Item> BuildDropItem(List<KeyValuePair<int, DropConfig>> dropList, int qualityRate)
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

                    Item item = ItemHelper.BuildItem((ItemType)config.ItemType, configId, qualityRate, 1);
                    list.Add(item);
                }
            }
            //TEST add equip
            //list.Add(ItemHelper.BuildItem(ItemType.Equip, 401, 1, 1)); 
            return list;
        }
    }
}