using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Game
{
    public class ItemHelper
    {
        public static Equip BuildEquip(int configId, int staticQuality, int qualityRate, int seed)
        {
            return EquipHelper.BuildEquip(configId, staticQuality, qualityRate, seed);
        }

        public static Item BuildItem(ItemType type, int configId, int qualityRate, long number)
        {
            return BuildItem(type, configId, qualityRate, number, -1);
        }

        public static Item BuildItem(ItemType type, int configId, int qualityRate, long number, int seed)
        {
            Item item = null;

            if (type == ItemType.Equip)
            {
                item = EquipHelper.BuildEquip(configId, 0, qualityRate, seed);
            }
            else if (type == ItemType.SkillBox)
            {
                item = new SkillBook(configId);
            }
            else if (type == ItemType.GiftPack)
            {
                item = new GiftPack(configId);
            }
            else if (type == ItemType.Material)
            {
                item = BuildMaterial(configId, number);
            }
            else if (type == ItemType.Exclusive)
            {
                item = ExclusiveHelper.Build(configId, seed);
            }
            else if (type == ItemType.GiftPackExclusive)
            {
                item = ExclusiveHelper.BuildByPack(configId);
            }
            else if (type == ItemType.GiftPackEquip)
            {
                item = EquipHelper.BuildByPack(configId);
            }
            else
            {
                item = new Item(configId);
                item.Type = type;
            }

            item.Count = number;

            return item;
        }

        public static Item BuildSoulRingShard(int quantity)
        {
            return BuildMaterial(SpecialId_SoulRingShard, quantity);
        }

        public static Item BuildRefineStone(int quantity)
        {
            return BuildMaterial(SpecialId_EquipRefineStone, quantity);
        }

        public static Item BuildGifPack(int configId)
        {
            return null;
        }

        public static Item BuildMaterial(int configId, long count)
        {
            Item item = new Item(configId);
            item.Type = ItemType.Material;
            item.Count = count;
            return item;
        }

        public static IEnumerable<Item> BurstMul(List<Item> items, int count, int qualityRate)
        {
            List<Item> newList = new List<Item>();
            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Item newItem = ItemHelper.BuildItem(items[i].Type, items[i].ConfigId, qualityRate, items[i].Count);

                    newList.Add(newItem);
                }
            }

            return newList;
        }

        public static int SpecialId_SoulRingShard = 4001; //魂环碎片
        public static int SpecialId_EquipRefineStone = 4002; //精炼石
        public static int SpecialId_Copy_Ticket = 4003; //装备副本卷
        public static int SpecialId_Boss_Ticket = 4004; //BOSS挑战卷
        public static int SpecialId_Exclusive_Stone = 4005; //专属碎片
        public static int SpecialId_Moon_Cake = 4006; //书页
        public static int SpecialId_Equip_Speical_Stone = 4007; //四格碎片
        public static int SpecialId_Wing_Stone = 4008; //凤凰之羽
        //public static int SpecialId_Exclusive_Core = 4009; //专属精华
        public static int SpecialId_Exclusive_Heart = 4010; //专属之心
        public static int SpecialId_Red_Stone = 4011;  //红装精华
        public static int SpecialId_Legacy_Stone = 4012; //传世精华
        public static int SpecialId_Legacy_Ticket = 4013; //传世挑战卷
        public static int SpecialId_Level_Stone = 4014; //等级丹
        public static int SpecialId_Red_Chip = 4015; //红装粉尘
        public static int SpecialId_Pill = 4016; //淬体丹
        public static int SpecialId_Pill_Ticket = 4017; //幻境挑战卷
        public static int SpecialId_Halidom_Chip = 4018; //遗物粉尘
        public static int SpecialId_Golden_Stone = 4019;  //金装精华


        public static int SpecialId_Card_Stone = 4101;
        public static int SpecialId_Skil_Advance = 4102;

        public static int SpecialId_Chunjie = 4111;

        public static int SpecailEquipRefreshId = 4201;
        //public static int[] SpecailEquipRefreshCount = { 10, 15, 20, 25, 30, 35, 40 };
    }
}
