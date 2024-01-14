using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class DropLimitConfigCategory
    {
    }

    public class DropLimitHelper
    {
        public static List<KeyValuePair<double, DropConfig>> Build(int type, double rateRise)
        {
            return Build(type, rateRise, null);
        }
        public static List<KeyValuePair<double, DropConfig>> Build(int type, double rateRise, Dictionary<int, double> rateData)
        {
            List<KeyValuePair<double, DropConfig>> list = new List<KeyValuePair<double, DropConfig>>();

            long time = DateTime.Now.Ticks;

            List<DropLimitConfig> drops = DropLimitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m =>
            m.Type == type &&
            DateTime.Parse(m.StartDate).Ticks <= time && time <= DateTime.Parse(m.EndDate).Ticks).ToList();

            foreach (DropLimitConfig dropLimit in drops)
            {
                DropConfig dropConfig = DropConfigCategory.Instance.Get(dropLimit.DropId);

                double rate = dropLimit.Rate;
                if (dropLimit.ShareRise > 0)
                {
                    rate = rate / rateRise;
                }

                if (dropLimit.StartRate > 0 && rateData != null)
                {
                    double currentRate = rateData[dropLimit.Id];

                    Debug.Log("Start Rate:" + dropLimit.Id + " ," + currentRate);

                    if (currentRate > dropLimit.StartRate)
                    {
                        rate = Math.Max(rate + dropLimit.StartRate - currentRate, 1);
                    }
                    else
                    {
                        continue;
                    }
                }

                list.Add(new KeyValuePair<double, DropConfig>(rate, dropConfig));
            }

            return list;
        }
    }

    public enum DropLimitType
    {
        Normal = 0,
        JieRi = 1,
        AnDian = 2,
        HeroPhatom = 99,
        Defend = 100,
    }
}