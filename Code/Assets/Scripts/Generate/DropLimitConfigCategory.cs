using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class DropLimitConfigCategory
    {
    }

    public class DropLimitHelper
    {
        public static List<Item> RandomItem(int rateIncrea)
        {
            List<Item> list = new List<Item>();

            long time = DateTime.Now.Ticks;

            List<DropLimitConfig> drops = DropLimitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => DateTime.Parse(m.StartDate).Ticks <= time &&
            time <= DateTime.Parse(m.EndDate).Ticks).ToList();

            foreach (DropLimitConfig dropLimit in drops)
            {
                int rate = dropLimit.Rate /rateIncrea;
                if (RandomHelper.RandomResult(rate))
                {
                    Item item = ItemHelper.BuildItem((ItemType)dropLimit.ItemType, dropLimit.ItemId, 1, 1);
                    list.Add(item);
                }
            }
            return list;
        }
    }
}