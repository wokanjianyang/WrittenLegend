using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class GiftPackConfigCategory
    {

    }

    public class GiftPackHelper
    {
        public static List<Item> BuildItems(int configId,ref int gold)
        {
            List<Item> items = new List<Item>();

            GiftPackConfig config = GiftPackConfigCategory.Instance.Get(configId);
            for (int i = 0; i < config.ItemIdList.Length; i++)
            {
                int itemId = config.ItemIdList[i];
                int quanlity = config.ItemQuanlityList[i];
                int type = config.ItemTypeList[i];

                Item item = null;

                switch (type)
                {
                    case (int)ItemType.Gold:
                        gold = quanlity;
                        break;
                    case (int)ItemType.Equip:
                        item = EquipHelper.BuildCustomEquip(itemId);
                        break;
                    case (int)ItemType.SkillBox:
                        item = SkillHelper.BuildItem(itemId);
                        break;
                    case (int)ItemType.GiftPack:
                        item = new GiftPack(itemId);
                        break;
                    default:
                        break;

                }

                if (item != null)
                {
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
