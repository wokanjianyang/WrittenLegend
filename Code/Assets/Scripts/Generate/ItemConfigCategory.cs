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

            Item item = new Item();

            item.Id = IdGenerater.Instance.GenerateId();
            item.ConfigId = ConfigId;
            item.Type = (ItemType)config.Type;
            item.Name = config.Name;
            item.Des = config.Name;
            item.Level = config.LevelRequired;
            item.Gold = config.Price;
            item.Quality = config.Quality;
            item.MaxNum = config.MaxNum;

            return item;
        }
    }
}