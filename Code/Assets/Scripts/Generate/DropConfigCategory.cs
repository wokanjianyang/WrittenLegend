using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class DropConfigCategory
    {
        public List<DropConfig> GetByMonsterId(int monsterId)
        {
            return this.dict.Where(m => m.Value.MonsterID == monsterId || m.Value.MonsterID==0).Select(m=>m.Value).ToList();
        }
    }

    public enum DropItemType {
        normal = 1,
        equip = 2,
    }

    public class DropHelper
    {
        public static List<Item> BuildDropItem(List<DropConfig> configs)
        {
            List<Item> list = new List<Item>();

            foreach (DropConfig config in configs)
            {
                for (int i = 0; i < config.ItemList.Length; i++)
                {
                    int count =0;

                    if (RandomHelper.RandomResult(config.Rate[i]))
                    {
                        Item item;

                        if (config.ItemType == (int)DropItemType.equip)
                        {
                            item = EquipHelper.BuildEquip(config.ItemList[i]);
                        }
                        else {
                            item = SkillHelper.BuildItem(config.ItemList[i]);
                        }                      
                        list.Add(item);
                        count++;
                    }

                    if (count >= config.RandomType)
                    {
                        break;
                    }
                }
            }
            return list;
        }
    }
}