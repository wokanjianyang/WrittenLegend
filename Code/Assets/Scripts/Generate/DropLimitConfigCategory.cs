using Game.Data;
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
        public static List<Item> Build(int type, int mapId, double rateRise, double modelRise, int qualityRate)
        {
            return Build(type, mapId, rateRise, modelRise, null, qualityRate);
        }

        public static List<Item> Build(int type, int mapId, double rateRise, double modelRise, Dictionary<int, double> rateData, int qualityRate)
        {
            User user = GameProcessor.Inst.User;

            List<Item> list = new List<Item>();

            long time = DateTime.Now.Ticks;

            int dzRate = user.GetDzRate();

            List<DropLimitConfig> drops = DropLimitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m =>
            m.Type == type && m.StartMapId <= mapId && mapId <= m.EndMapId
            && DateTime.Parse(m.StartDate).Ticks <= time && time <= DateTime.Parse(m.EndDate).Ticks).ToList();

            foreach (DropLimitConfig dropLimit in drops)
            {
                int dropLimitId = dropLimit.Id;
                DropData dropData = user.DropDataList.Where(m => m.DropLimitId == dropLimitId).FirstOrDefault();
                if (dropData == null)
                {
                    dropData = new DropData(dropLimitId);
                    dropData.Init(user.DeviceId.GetHashCode() + dropLimitId);
                    user.DropDataList.Add(dropData);
                }

                

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

                    int dropId = dropLimit.DropId;
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

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