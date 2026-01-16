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
            return EquipHelper.BuildEquip(configId, staticQuality, qualityRate, seed, RuleType.Normal);
        }

        public static Item BuildItem(ItemType type, int configId, int qualityRate, long number)
        {
            return BuildItem(type, configId, qualityRate, number, -1, RuleType.Normal);
        }

        public static Item BuildItem(ItemType type, int configId, int qualityRate, long number, RuleType ruleType)
        {
            return BuildItem(type, configId, qualityRate, number, -1, ruleType);
        }

        public static Item BuildItem(ItemType type, int configId, int qualityRate, long number, int seed, RuleType ruleType)
        {
            Item item = null;

            if (type == ItemType.Equip)
            {
                item = EquipHelper.BuildEquip(configId, 0, qualityRate, seed, ruleType);
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
                item = ExclusiveHelper.Build(configId, qualityRate, seed);
            }
            else if (type == ItemType.GiftPackExclusive)
            {
                item = ExclusiveHelper.BuildByPack(configId);
            }
            else if (type == ItemType.GiftPackEquip)
            {
                item = EquipHelper.BuildByPack(configId);
            }
            else if (type == ItemType.GiftPackPet)
            {
                item = PetConfigCategory.Instance.BuildByPack(configId);
            }
            else if (type == ItemType.GiftPackShengxiao)
            {
                item = ShengxiaoConfigCategory.Instance.BuildByPack(configId);
            }
            else if (type == ItemType.Pet)
            {
                item = PetConfigCategory.Instance.BuildPet(configId);
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

        public static Item BuildRefineStone(long quantity)
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

        public static IEnumerable<Item> BurstMul(List<Item> items, int count, int qualityRate, RuleType ruleType)
        {
            List<Item> newList = new List<Item>();
            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Item newItem = ItemHelper.BuildItem(items[i].Type, items[i].ConfigId, qualityRate, items[i].Count, ruleType);

                    newList.Add(newItem);
                }
            }

            return newList;
        }

        public static IEnumerable<Item> BurstMul(List<Item> items, int count, double qualityRate, RuleType ruleType, int maxQuality)
        {
            List<Item> newList = new List<Item>();
            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < items.Count; i++)
                {

                    if (items[i].Type == ItemType.Shengxiao)
                    {
                        Item newItem = ShengxiaoConfigCategory.Instance.Build(items[i].ConfigId, qualityRate, maxQuality, 0);

                        newList.Add(newItem);
                    }
                    else
                    {
                        Item newItem = ItemHelper.BuildItem(items[i].Type, items[i].ConfigId, (int)qualityRate, items[i].Count, ruleType);

                        newList.Add(newItem);
                    }
                }
            }

            return newList;
        }


        public static int SpecialId_SoulRingShard = 4001; //魂环碎片
        public static int SpecialId_EquipRefineStone = 4002; //精炼石
        public static int SpecialId_Copy_Ticket = 4003; //装备副本卷
        public static int SpecialId_Boss_Ticket = 4004; //BOSS挑战卷
        public static int SpecialId_Exclusive_Stone = 4005; //专属碎片
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
        public static int SpecialId_Pill2 = 4033; //行气丹
        public static int SpecialId_Pill3 = 4040; //炼神丹
        public static int SpecialId_Fashion = 4034; //行气丹
        public static int SpecialId_Exclusive_Golden = 4035; //传奇精华
        public static int SpecialId_Exclusive_Dark = 4039; //不朽精华
        public static int SpecialId_Exclusive_New = 4037; //永恒精华

        public static int SpecialId_Equip_Hundun = 4038; //混沌装备精华

        public static int SpecialId_Pill_Ticket = 4017; //幻境挑战卷
        public static int SpecialId_Halidom_Chip = 4018; //遗物粉尘
        public static int SpecialId_Golden_Stone = 4019;  //金装精华
        public static int SpecialId_Talent_Book = 4020; //天赋书
        public static int SpecialId_Reform_Stone = 4021; //改造石
        public static int SpecialId_Pet_Exp = 4022; //魂宠口粮
        public static int SpecialId_Dark_Stone = 4026;//暗金精华
        public static int SpecialId_Stone_Set = 4027; //魂宠口粮

        public static int SpecialId_Card_Stone = 4101;

        public static int SpecialId_Chunjie = 4111;
  

        public static int SpecialId_Shuye1 = 4006; //书页
        public static int SpecialId_Shuye2 = 4102; //高级书页
        public static int SpecialId_Shuye3 = 4112; //超级书页

        public static int Speical_Festive_Attr = 4113; //快乐精粹

        public static int SpecailEquipRefreshId = 4201; //橙装精华

        public static int[] Specail_Pet_Layer = { 4023, 4024, 4025 };

        public static int Specail_Pet_Speical = 4041; //暗金魂心
        public static int Specail_Shengxiao = 4042; //生肖精华
        public static int Specail_Shengxiao1 = 4043; //生肖本源
        public static int Specail_Shengxiao2 = 4044; //生肖核心
    }
}
