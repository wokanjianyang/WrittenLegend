using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ExclusiveDevourConfigCategory
    {
        public ExclusiveDevourConfig GetByCycleAndLevel(int cycle, int level)
        {
            return this.list.Where(m => m.Cycle == cycle && m.Level == level).First();
        }

        public Dictionary<int, int> GetUseList(int cycle, int level)
        {
            Dictionary<int, int> useList = new Dictionary<int, int>();

            List<ExclusiveDevourConfig> configs = this.list.Where(m => m.Cycle == cycle && m.Level < level).ToList();

            foreach (ExclusiveDevourConfig config in configs)
            {
                for (int i = 0; i < config.ItemIdList.Length; i++)
                {
                    int key = config.ItemIdList[i];
                    int count = config.ItemCountList[i];

                    if (!useList.ContainsKey(key))
                    {
                        useList[key] = 0;
                    }

                    useList[key] += count;
                }
            }

            return useList;
        }
    }



}