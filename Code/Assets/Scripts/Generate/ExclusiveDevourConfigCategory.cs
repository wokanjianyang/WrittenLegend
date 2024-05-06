using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ExclusiveDevourConfigCategory
    {
        public Dictionary<int, int> GetUseList(int level)
        {
            Dictionary<int, int> useList = new Dictionary<int, int>();

            List<ExclusiveDevourConfig> configs = this.list.Where(m => m.Level < level).ToList();

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