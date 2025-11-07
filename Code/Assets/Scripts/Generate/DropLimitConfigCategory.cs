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

        public bool CheckIsTime()
        {
            long time = DateTime.Now.Ticks;
            DropLimitConfig dropLimit = DropLimitConfigCategory.Instance.Get(1);
            if ((DateTime.Parse(dropLimit.StartDate).Ticks <= time && time <= DateTime.Parse(dropLimit.EndDate).Ticks))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DropLimitHelper
    {
        public static List<Item> BuildJieRi(double modelRise)
        {
            List<Item> list = new List<Item>();

            long time = DateTime.Now.Ticks;

            //int dropType = (int)DropLimitType.JieRi;
            ////不检测limitid
            //DropLimitConfig dropLimit = DropLimitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m =>
            //m.Type == dropType && DateTime.Parse(m.StartDate).Ticks <= time && time <= DateTime.Parse(m.EndDate).Ticks).FirstOrDefault();

            //如果节日中，或者是周末，则使用Drop1，否则使用Drop2
            DropLimitConfig dropLimit = DropLimitConfigCategory.Instance.Get(1);
            if ((DateTime.Parse(dropLimit.StartDate).Ticks <= time && time <= DateTime.Parse(dropLimit.EndDate).Ticks)
                || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                //Debug.Log("drop1");
            }
            else
            {
                //Debug.Log("drop2");
                dropLimit = DropLimitConfigCategory.Instance.Get(2);
            }

            double rate = dropLimit.Rate;
            rate = rate / modelRise;

            if (RandomHelper.RandomResult(rate))
            {
                int dropId = dropLimit.DropId;
                DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

                int configId = dropConfig.ItemIdList[0];

                Item item = ItemHelper.BuildItem((ItemType)dropConfig.ItemType, configId, 1, dropConfig.Quantity, 0);
                list.Add(item);
            }

            return list;
        }

        public static List<Item> Build(int type, int mapId, double rateRise, double modelRise, int limit, double countRise)
        {
            return Build(type, mapId, rateRise, modelRise, limit, countRise, 1);
        }

        public static List<Item> Build(int type, int mapId, double rateRise, double modelRise, int limit, double countRise, double dropFinal)
        {
            User user = GameProcessor.Inst.User;

            List<Item> list = new List<Item>();

            long time = DateTime.Now.Ticks;

            //不检测limitid
            List<DropLimitConfig> drops = DropLimitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m =>
            m.Type == type && m.StartMapId <= mapId && mapId <= m.EndMapId
            && DateTime.Parse(m.StartDate).Ticks <= time && time <= DateTime.Parse(m.EndDate).Ticks).ToList();

            if (mapId >= 1105 && modelRise > 5)
            {
                modelRise = 5;
            }

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

                if (dropLimit.StartRate > 0 || dropLimit.EndRate > 0 || dropLimit.MinRate > 0) //有保底机制的
                {
                    dropData.Number += countRise * dropFinal;

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
                        //Debug.Log("Start End Rate:" + dropLimit.Id + " ," + rate);
                    }

                    if (dropLimit.MinRate > 0 && dropData.Number >= dropLimit.Rate)
                    {
                        rate = dropLimit.MinRate;

                        //Debug.Log("Drop Limit Rate:" + dropLimit.Id + " ," + rate);
                    }
                }

                if (dropLimitId >= 2005 && modelRise > 10)
                {
                    modelRise = 10;
                }

                rate = rate / modelRise / dropFinal;

                if (RandomHelper.RandomResult(rate))
                {
                    dropData.Number = 0;
                    dropData.Seed++;

                    int seed = TimeHelper.TodaySeed() + dropData.Seed;


                    int dropId = dropLimit.DropId;
                    DropConfig dropConfig = DropConfigCategory.Instance.Get(dropId);

                    int index = RandomHelper.RandomNumber(seed, 0, dropConfig.ItemIdList.Length);
                    int configId = dropConfig.ItemIdList[index];

                    Item item = ItemHelper.BuildItem((ItemType)dropConfig.ItemType, configId, 1, dropConfig.Quantity, seed, RuleType.Normal);
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