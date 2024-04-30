using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class DropLimitConfigCategory
    {
        public List<DropLimitConfig> GetByMapId(int type, int mapId)
        {
            long time = DateTime.Now.Ticks;

            List<DropLimitConfig> drops = this.list.Where(m => m.Type == type && m.StartMapId <= mapId && mapId <= m.EndMapId
            && DateTime.Parse(m.StartDate).Ticks <= time && time <= DateTime.Parse(m.EndDate).Ticks).ToList();
            return drops;
        }
    }

    public class DropLimitHelper
    {
        public static List<Item> Build(int type, int mapId, double rateRise, double modelRise, int qualityRate, double countRise)
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

                if (dropData.Number > 0)
                {
                    //Debug.Log("Map Limit Drop: " + dropLimitId + " :" + dropData.Number);
                }

                double rate = dropLimit.Rate;

                if (dropLimit.ShareRise > 0)
                {
                    rate = rate / rateRise;
                }

                if (dropLimit.StartRate > 0) //有保底机制的
                {
                    dropData.Number += countRise * dzRate;

                    if (dropData.Number > dropLimit.StartRate)
                    {
                        rate = Math.Max(rate + dropLimit.StartRate - dropData.Number, 1);

                        //Debug.Log("Start Drop Rate:" + dropId + " ," + rate);
                    }
                    else
                    {
                        //Debug.Log("Start Current Rate:" + dropId + " ," + currentRate);
                        continue;
                    }
                }
                if (dropLimitId == 2005 && modelRise > 10)
                {
                    modelRise = 10;
                }

                rate = rate / modelRise;

                if (RandomHelper.RandomResult(rate))
                {
                    dropData.Number = 0;
                    dropData.Seed = AppHelper.RefreshSeed(dropData.Seed);


                    int dropId = dropLimit.DropId;
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

                    int index = RandomHelper.RandomNumber(dropData.Seed, 0, dropConfig.ItemIdList.Length);
                    int configId = dropConfig.ItemIdList[index];

                    if (dropLimit.ShareDz <= 0)
                    {
                        dzRate = 1;
                    }

                    Item item = ItemHelper.BuildItem((ItemType)dropConfig.ItemType, configId, qualityRate, dropConfig.Quantity * dzRate, dropData.Seed);
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