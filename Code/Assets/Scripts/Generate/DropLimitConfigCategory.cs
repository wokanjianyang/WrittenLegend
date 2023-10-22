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
        public static List<KeyValuePair<int, DropConfig>> Build(double rateRise)
        {
            List<KeyValuePair<int, DropConfig>> list = new List<KeyValuePair<int, DropConfig>>();

            long time = DateTime.Now.Ticks;

            List<DropLimitConfig> drops = DropLimitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => DateTime.Parse(m.StartDate).Ticks <= time &&
            time <= DateTime.Parse(m.EndDate).Ticks).ToList();

            foreach (DropLimitConfig dropLimit in drops)
            {
                DropConfig dropConfig = DropConfigCategory.Instance.Get(dropLimit.DropId);

                int rate = dropLimit.Rate;
                if (dropLimit.ShareRise > 0)
                {
                    rate = (int)(rate / rateRise);
                }
                list.Add(new KeyValuePair<int, DropConfig>(rate, dropConfig));
            }

            return list;
        }
    }
}