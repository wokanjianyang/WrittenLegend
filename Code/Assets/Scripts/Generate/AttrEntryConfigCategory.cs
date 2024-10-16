using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class AttrEntryConfigCategory
    {
        public List<KeyValuePair<int, long>> Build(int part, int level, int quality, int role, int seed)
        {
            List<KeyValuePair<int, long>> rsList = new List<KeyValuePair<int, long>>();

            List<AttrEntryConfig> configs = list.FindAll(m =>
            m.PartList.Contains(part)
            && m.StartLevel <= level && level <= m.EndLevel
            && m.StartQuality <= quality && quality <= m.EndQuality
            && (m.Role == role || m.Role == 0));

            if (configs.Count <= 0)
            {
                return rsList;
            }

            for (int i = 0; i < quality; i++)
            {
                seed = AppHelper.RefreshDaySeed(seed);

                List<int> excludeList = GetExcludeList(rsList);

                var fcList = configs.Where(m => !excludeList.Contains(m.Id)).ToList();

                int rd = RandomHelper.RandomNumber(seed, 0, fcList.Count);

                AttrEntryConfig config = fcList[rd];

                long attrValue = 0;

                seed = AppHelper.RefreshDaySeed(seed);
                attrValue = RandomHelper.RandomNumber(seed, config.MinValue, config.MaxValue + 1);

                rsList.Add(new KeyValuePair<int, long>(config.AttrId, attrValue));
            }

            return rsList;
        }

        public List<KeyValuePair<int, long>> Build(int part, int level, int quality, int role)
        {
            List<KeyValuePair<int, long>> rsList = new List<KeyValuePair<int, long>>();

            List<AttrEntryConfig> configs = list.FindAll(m =>
            m.PartList.Contains(part)
            && m.StartLevel <= level && level <= m.EndLevel
            && m.StartQuality <= quality && quality <= m.EndQuality
            && (m.Role == role || m.Role == 0));

            if (configs.Count <= 0)
            {
                return rsList;
            }

            for (int i = 0; i < quality; i++)
            {
                List<int> excludeList = GetExcludeList(rsList);

                var fcList = configs.Where(m => !excludeList.Contains(m.Id)).ToList();

                int rd = RandomHelper.RandomNumber(0, fcList.Count);

                AttrEntryConfig config = fcList[rd];

                long attrValue = 0;

                attrValue = RandomHelper.RandomNumber(config.MinValue, config.MaxValue + 1);

                rsList.Add(new KeyValuePair<int, long>(config.AttrId, attrValue));
            }

            return rsList;
        }

        public List<KeyValuePair<int, long>> BuildNew(int part, int level, int quality, int role, RandomRecord record)
        {
            List<KeyValuePair<int, long>> rsList = new List<KeyValuePair<int, long>>();

            List<AttrEntryConfig> configs = list.FindAll(m =>
            m.PartList.Contains(part)
            && m.StartLevel <= level && level <= m.EndLevel
            && m.StartQuality <= quality && quality <= m.EndQuality
            && (m.Role == role || m.Role == 0));

            if (configs.Count <= 0)
            {
                return rsList;
            }

            for (int i = 0; i < quality; i++)
            {
                List<int> excludeList = GetExcludeList(rsList);

                var fcList = configs.Where(m => !excludeList.Contains(m.Id)).ToList();

                int rd = RandomTableHelper.Instance().Random(0, fcList.Count, record);

                AttrEntryConfig config = fcList[rd];

                long attrValue = 0;

                attrValue = RandomTableHelper.Instance().Random(config.MinValue, config.MaxValue + 1, record);

                rsList.Add(new KeyValuePair<int, long>(config.AttrId, attrValue));
            }

            return rsList;
        }

        private List<int> GetExcludeList(List<KeyValuePair<int, long>> rsList)
        {
            List<int> excludeList = new List<int>();

            foreach (AttrEntryConfig config in list)
            {
                int count = rsList.Where(m => m.Key == config.AttrId).Count();

                if (count >= config.MaxCount)
                {
                    excludeList.Add(config.Id);

                    //Debug.Log("Exclued id :" + config.Id + " count:" + count);
                }
            }
            return excludeList;
        }

        public List<AttrEntryConfig> GetRedAttrList()
        {
            return this.list.Where(m => m.Type == 1 && m.EndQuality == 6).ToList();
        }

        public AttrEntryConfig GetRedConfig(int attrId)
        {
            return this.list.Where(m => m.Type == 1 && m.EndQuality == 6 && m.AttrId == attrId).FirstOrDefault();
        }
    }
}
