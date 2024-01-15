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
        public static List<KeyValuePair<double, DropConfig>> Build(int type, double rateRise, double modelRise)
        {
            return Build(type, rateRise, modelRise, null);
        }
        public static List<KeyValuePair<double, DropConfig>> Build(int type, double rateRise, double modelRise, Dictionary<int, double> rateData)
        {
            List<KeyValuePair<double, DropConfig>> list = new List<KeyValuePair<double, DropConfig>>();

            long time = DateTime.Now.Ticks;

            List<DropLimitConfig> drops = DropLimitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m =>
            m.Type == type &&
            DateTime.Parse(m.StartDate).Ticks <= time && time <= DateTime.Parse(m.EndDate).Ticks).ToList();

            foreach (DropLimitConfig dropLimit in drops)
            {
                int dropId = dropLimit.DropId;
                DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

                double rate = dropLimit.Rate;

                if (dropLimit.ShareRise > 0)
                {
                    rate = rate / rateRise;
                }

                if (dropLimit.StartRate > 0 && rateData != null)
                {
                    double currentRate = rateData[dropId];

                    //Debug.Log("Start Rate:" + dropId + " ," + currentRate);

                    if (currentRate > dropLimit.StartRate)
                    {
                        rate = Math.Max(rate + dropLimit.StartRate - currentRate, 1);
                    }
                    else
                    {
                        continue;
                    }
                }

                rate = rate / modelRise;

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