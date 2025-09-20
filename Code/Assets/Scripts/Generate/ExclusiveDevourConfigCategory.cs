using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ExclusiveDevourConfigCategory
    {
        public ExclusiveDevourConfig GetByCycleAndLevel(int cycle, int layer)
        {
            return this.list.Where(m => m.Cycle == cycle && m.Layer == layer).First();
        }

        public Dictionary<int, int> GetUseList(int cycle, int layer, int level)
        {
            Dictionary<int, int> useList = new Dictionary<int, int>();

            List<ExclusiveDevourConfig> configs = this.list.Where(m => m.Cycle == cycle && m.Layer < layer).ToList();

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

            if (level > 0)
            {
                for (int i = 0; i < configs[0].UpItemIdList.Length; i++)
                {
                    int key = configs[0].UpItemIdList[i];
                    int count = configs[0].UpItemCountList[i];

                    if (!useList.ContainsKey(key))
                    {
                        useList[key] = 0;
                    }

                    useList[key] += count * level;
                }
            }

            return useList;
        }
    }



}