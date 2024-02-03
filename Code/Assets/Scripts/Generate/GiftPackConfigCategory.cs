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
        public static List<Item> BuildItems(int configId, ref int gold)
        {
            List<Item> items = new List<Item>();

            GiftPackConfig config = GiftPackConfigCategory.Instance.Get(configId);
            for (int i = 0; i < config.ItemIdList.Length; i++)
            {
                int itemId = config.ItemIdList[i];
                int count = config.ItemCountList[i];
                ItemType type = (ItemType)config.ItemTypeList[i];

                Item item = null;

                if (type == ItemType.Gold)
                {
                    gold = count;
                }
                else
                {
                    item = ItemHelper.BuildItem(type, itemId, 1, count);
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
