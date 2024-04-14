using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class InfiniteDropConfigCategory
    {
        public List<int> GetAllDropIdList()
        {
            int maxLevel = InfiniteConfigCategory.Instance.GetMaxLevel();

            List<int> rates = new List<int>();

            for (int i = 1; i <= maxLevel; i++)
            {
                List<InfiniteDropConfig> dropConfigs = this.GetLevelList(i, rates);

                int index = RandomHelper.RandomNumber(0, dropConfigs.Count);

                rates.Add(dropConfigs[index].DropId);
            }

            return rates;
        }

        private List<InfiniteDropConfig> GetLevelList(long level, List<int> excludeList)
        {
            List<InfiniteDropConfig> configs = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).ToList();

            List<InfiniteDropConfig> list = new List<InfiniteDropConfig>();

            foreach (InfiniteDropConfig config in configs)
            {
                int total = excludeList.Select(m => m == config.DropId).Count();
                if (config.Max > total)
                {
                    list.Add(config);
                }
            }
            return list;
        }
    }

}
