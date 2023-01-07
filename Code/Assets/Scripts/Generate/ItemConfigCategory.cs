using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ItemConfigCategory
    {
    }

    public class ItemHelper
    {
        public static Item BuildItem(int ConfigId)
        {
            ItemConfig config = ItemConfigCategory.Instance.Get(ConfigId);

            Item item = new Item(ConfigId);

            return item;
        }
    }
}