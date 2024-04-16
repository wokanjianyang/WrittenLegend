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

                rates.Add(RandomDropId(dropConfigs));
            }

            return rates;
        }

        private int RandomDropId(List<InfiniteDropConfig> dropConfigs)
        {
            int total = dropConfigs.Select(m => m.Rate).Sum();
            int rd = RandomHelper.RandomNumber(1, total + 1);

            int endRate = 0;
            for (int i = 0; i < dropConfigs.Count; i++)
            {
                endRate += dropConfigs[i].Rate;

                if (rd <= endRate)
                {
                    return dropConfigs[i].DropId;
                }
            }

            return -1;
        }

        private List<InfiniteDropConfig> GetLevelList(long level, List<int> excludeList)
        {
            List<InfiniteDropConfig> configs = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level && level % m.RateLevel == 0).ToList();

            List<InfiniteDropConfig> list = new List<InfiniteDropConfig>();

            foreach (InfiniteDropConfig config in configs)
            {
                int total = excludeList.Where(m => m == config.DropId).Count();

                if (config.DropId >= 180001 && config.DropId <= 180100) //ÉñÆ÷
                {
                    ArtifactConfig artifactConfig = ArtifactConfigCategory.Instance.GetByItemId(config.DropId);
                    int atLevel = GameProcessor.Inst.User.GetArtifactLevel(artifactConfig.Id);

                    if (total + atLevel >= artifactConfig.MaxCount)
                    {
                        continue;
                    }
                }

                if (config.Max > total)
                {
                    list.Add(config);
                }
            }
            return list;
        }
    }

}
