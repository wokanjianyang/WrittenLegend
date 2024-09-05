using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class DefendDropConfigCategory
    {
        public List<int> GetAllDropIdList(int layer)
        {
            List<int> rates = new List<int>();

            for (int i = 1; i <= 100; i++)
            {
                List<DefendDropConfig> dropConfigs = this.GetLevelList(layer, i, rates);

                rates.Add(RandomDropId(dropConfigs));
            }

            return rates;
        }

        private int RandomDropId(List<DefendDropConfig> dropConfigs)
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

        private List<DefendDropConfig> GetLevelList(int layer, long level, List<int> excludeList)
        {
            List<DefendDropConfig> configs = this.list.Where(m => m.Layer == layer && m.StartLevel <= level && m.EndLevel >= level && level % m.RateLevel == 0).ToList();

            List<DefendDropConfig> list = new List<DefendDropConfig>();

            foreach (DefendDropConfig config in configs)
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