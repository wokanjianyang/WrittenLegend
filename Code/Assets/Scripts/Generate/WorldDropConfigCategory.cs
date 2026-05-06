using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class WorldDropConfigCategory
    {
        public List<int> GetAllDropIdList(int mapId, int seed)
        {
            List<int> rates = new List<int>();

            //Dictionary<int, int> dict = new Dictionary<int, int>();

            int stoneDropId = mapId <= 5 ? 0 : -1;
            List<WorldDropConfig> dropConfigs = this.list.Where(m => (m.MapId == mapId || m.MapId == stoneDropId)).ToList();

            for (int level = 1; level <= ConfigHelper.MaxWorld; level++)
            {
                if (seed > 0)
                {
                    seed++;
                }

                List<WorldDropConfig> temps = dropConfigs.Where(m => m.StartLevel <= level && m.EndLevel >= level
                && ((level - m.StartLevel) % m.RateLevel == 0)
                && (m.ExcludeStart > level || m.ExcludeLevel == 0 || level % m.ExcludeLevel != 0)).ToList();

                WorldDropConfig config = RandomDropId(temps, seed);

                rates.Add(config.ItemId);

                //if (!dict.ContainsKey(config.Id))
                //{
                //    dict[config.Id] = 0;
                //}

                //dict[config.Id]++;
            }

            //string text = "";
            //var dd = dict.OrderBy(m => m.Key).ToList();
            //foreach (var sp in dd)
            //{
            //    if (sp.Key > 100)
            //    {
            //        text += sp.Key + "-" + sp.Value + " £¬ ";
            //    }
            //}

            //Debug.Log(text);

            return rates;
        }

        private WorldDropConfig RandomDropId(List<WorldDropConfig> dropConfigs, int seed)
        {
            int total = dropConfigs.Select(m => m.Rate).Sum();

            int rd = RandomHelper.RandomNumber(seed, 1, total + 1);

            int endRate = 0;
            for (int i = 0; i < dropConfigs.Count; i++)
            {
                endRate += dropConfigs[i].Rate;

                if (rd <= endRate)
                {
                    return dropConfigs[i];
                }
            }

            return null;
        }
    }
}
