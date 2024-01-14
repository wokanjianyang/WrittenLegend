using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Game
{
    public class ItemHelper
    {
        public static Equip BuildEquip(int configId, int staticQuality, int qualityRate)
        {
            return EquipHelper.BuildEquip(configId, staticQuality, qualityRate);
        }

        public static Item BuildItem(ItemType type, int configId, int qualityRate, int number)
        {
            Item item = null;

            if (type == ItemType.Equip)
            {
                item = EquipHelper.BuildEquip(configId, 0, qualityRate);
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
            else if (type == ItemType.GoldPack || type == ItemType.ExpPack || type == ItemType.Ticket)
            {
                item = new Item(configId);
                item.Type = type;
            }
            else if (type == ItemType.Exclusive)
            {
                item = ExclusiveHelper.Build(configId);
            }
            else if (type == ItemType.GiftPackExclusive)
            {
                item = ExclusiveHelper.BuildByPack(configId);
            }
            else if (type == ItemType.Card)
            {
                item = new Item(configId);
                item.Type = ItemType.Card;
            }
            else if (type == ItemType.Fashion)
            {
                item = new Item(configId);
                item.Type = ItemType.Fashion;
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

        public static Item BuildMaterial(int configId, int quanlity)
        {
            Item item = new Item(configId);
            item.Type = ItemType.Material;
            item.Count = quanlity;
            return item;
        }

        public static int SpecialId_SoulRingShard = 4001;
        public static int SpecialId_EquipRefineStone = 4002;
        public static int SpecialId_Copy_Ticket = 4003;
        public static int SpecialId_Boss_Ticket = 4004;
        public static int SpecialId_Exclusive_Stone = 4005;
        public static int SpecialId_Moon_Cake = 4006;
        public static int SpecialId_Equip_Speical_Stone = 4007;
        public static int SpecialId_Wing_Stone = 4008;

        public static int SpecialId_Card_Stone = 4101;
    }
}
