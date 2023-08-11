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
        public static List<Item> TowerEquip(long fl, int equipLevel)
        {
            List<Item> items = new List<Item>();

            if (fl % 20000 == 0)
            {
                //2w层掉落当前等级橙装
                items.AddRange(DropHelper.RandomLevelEquip(equipLevel, 5));
            }
            if (fl % 1500 == 0)
            {
                //2000层掉落当前等级紫装
                items.AddRange(DropHelper.RandomLevelEquip(equipLevel, 4));
            }
            if (fl % 1000 == 0)
            {
                int fourLevel = 100;
                if (fl > 500000)  //50w以上，掉落2级四格,以下掉落3级4格
                {
                    fourLevel = 200;
                }
                int rd = RandomHelper.RandomNumber(1, 5);
                items.Add(ItemHelper.BuildEquip(rd * fourLevel + 1, 1, 1));
            }

            if (fl % 30 == 0)
            {
                //每30层掉落当前等级随机装备
                items.AddRange(DropHelper.RandomLevelEquip(equipLevel, 0));
            }
            return items;
        }

        public static List<Item> RandomLevelEquip(int equipLevel, int quality)
        {
            List<Item> list = new List<Item>();

            List<DropConfig> drops = DropConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Level == equipLevel && m.ItemType == 2).ToList(); //获取当前等级的装备
            if (drops.Count > 0)
            {
                int[] ids = drops[0].ItemIdList;
                if (ids != null && ids.Length > 0)
                {
                    int index = RandomHelper.RandomNumber(0, ids.Length);

                    Item item = ItemHelper.BuildEquip(ids[index], quality, 1);
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