using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

namespace Game
{
    public class ItemHelper
    {
        public static Item BuildItem(ItemType type, int configId, int quality, int number)
        {
            Item item = null;

            if (type == ItemType.Equip)
            {
                item = EquipHelper.BuildEquip(configId, quality);
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

            return item;
        }

        public static Item BuildSoulRingShard(int quantity)
        {
            return BuildMaterial(SpecialId_SoulRingShard, quantity);
        }


        public static Item BuildGifPack(int configId)
        {
            return null;
        }

        public static Item BuildMaterial(int configId, int quanlity)
        {
            Item item = new Item(configId);
            item.Quantity = quanlity;
            return item;
        }

        public static int SpecialId_SoulRingShard = 4001;
    }
}
