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
        public static List<Item> Build(int type, int mapId, double rateRise, double modelRise, int limit, double countRise)
        {
            User user = GameProcessor.Inst.User;

            List<Item> list = new List<Item>();

            long time = DateTime.Now.Ticks;

            //不检测limitid
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

                if (dropLimit.StartRate > 0 || dropLimit.EndRate > 0) //有保底机制的
                {
                    dropData.Number += countRise;

                    //if (dropLimit.Id >= 2005)
                    //{
                    //    Debug.Log("Start Drop Rate:" + dropLimit.Id + " ," + dropData.Number);
                    //}

                    if (dropLimit.StartRate > 0 && dropData.Number < dropLimit.StartRate)
                    {
                        continue;
                    }

                    if (dropLimit.EndRate > 0 && dropData.Number >= dropLimit.EndRate)
                    {
                        rate = 1;
                        Debug.Log("Start End Rate:" + dropLimit.Id + " ," + rate);
                    }
                }

                if (dropLimitId >= 2005 && modelRise > 10)
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

                    Item item = ItemHelper.BuildItem((ItemType)dropConfig.ItemType, configId, 1, dropConfig.Quantity, dropData.Seed);
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
        Pill = 101,
    }
}