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
        public static List<Item> Build(int type, double rateRise, double modelRise, int dzRate, int qualityRate)
        {
            return Build(type, rateRise, modelRise, null, dzRate, qualityRate);
        }
        public static List<Item> Build(int type, double rateRise, double modelRise, Dictionary<int, double> rateData, int dzRate, int qualityRate)
        {
            List<Item> list = new List<Item>();

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
                    double currentRate = rateData[dropLimit.Id];

                    if (currentRate > dropLimit.StartRate)
                    {
                        rate = Math.Max(rate + dropLimit.StartRate - currentRate, 1);

                        //Debug.Log("Start Drop Rate:" + dropId + " ," + rate);
                    }
                    else
                    {
                        //Debug.Log("Start Current Rate:" + dropId + " ," + currentRate);
                        continue;
                    }
                }

                rate = rate / modelRise;


                if (RandomHelper.RandomResult(rate))
                {
                    if (rateData != null && rateData.ContainsKey(dropLimit.Id))
                    {
                        rateData[dropLimit.Id] = 0;
                        //Debug.Log("drop id " + config.Id + " ±£µ×¹éÁã ");
                    }

                    int index = RandomHelper.RandomNumber(0, dropConfig.ItemIdList.Length);
                    int configId = dropConfig.ItemIdList[index];

                    if (dropLimit.ShareDz <= 0)
                    {
                        dzRate = 1;
                    }

                    Item item = ItemHelper.BuildItem((ItemType)dropConfig.ItemType, configId, qualityRate, dropConfig.Quantity * dzRate);
                    list.Add(item);
                }
            }

            return list;
        }
    }

    public enum DropLimitType
    {
        Normal = 0,
        JieRi = 1,
        AnDian = 2,
        Map = 98,
        HeroPhatom = 99,
        Defend = 100,
    }
}