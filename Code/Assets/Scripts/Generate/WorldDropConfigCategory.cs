using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class WorldDropConfigCategory
    {
        public List<int> GetAllDropIdList(int mapId, int seed)
        {
            List<int> rates = new List<int>();

            for (int level = 1; level <= ConfigHelper.MaxWorld; level++)
            {
                if (seed > 0)
                {
                    seed++;
                }

                List<WorldDropConfig> dropConfigs = this.list.Where(m => (m.MapId == mapId || m.MapId == 0)
                && m.StartLevel <= level && m.EndLevel >= level
                && ((level - m.StartLevel) % m.RateLevel == 0)
                && (m.ExcludeStart > level || m.ExcludeLevel == 0 || level % m.ExcludeLevel != 0)).ToList();

                rates.Add(RandomDropId(dropConfigs, seed));
            }

            return rates;
        }

        private int RandomDropId(List<WorldDropConfig> dropConfigs, int seed)
        {
            int total = dropConfigs.Select(m => m.Rate).Sum();

            int rd = RandomHelper.RandomNumber(seed, 1, total + 1);

            int endRate = 0;
            for (int i = 0; i < dropConfigs.Count; i++)
            {
                endRate += dropConfigs[i].Rate;

                if (rd <= endRate)
                {
                    return dropConfigs[i].ItemId;
                }
            }

            return -1;
        }
    }
}
