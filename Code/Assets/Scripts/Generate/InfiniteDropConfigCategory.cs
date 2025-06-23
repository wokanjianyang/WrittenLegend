using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class InfiniteDropConfigCategory
    {
        public InfiniteDropConfig GetConfig(int dropId, long level)
        {
            return this.list.Where(m => m.DropId == dropId && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
        }

        public List<int> GetAllDropIdList()
        {
            int maxLevel = ConfigHelper.Infinit_Max;

            List<int> rates = new List<int>();

            Dictionary<int, int> dict = new Dictionary<int, int>();

            List<InfiniteDropConfig> dropConfigs = new List<InfiniteDropConfig>();

            foreach (InfiniteDropConfig config in this.list)
            {
                if (config.DropId >= 180001 && config.DropId <= 180100) //神器
                {
                    ArtifactConfig artifactConfig = ArtifactConfigCategory.Instance.GetByItemId(config.DropId);
                    int atLevel = GameProcessor.Inst.User.GetArtifactLevel(artifactConfig.Id);

                    if (atLevel >= artifactConfig.MaxCount)
                    {
                        continue;
                    }
                }

                dropConfigs.Add(config);
            }


            for (int level = 1; level <= maxLevel; level++)
            {
                dropConfigs.RemoveAll(m => m.EndLevel < level);

                List<InfiniteDropConfig> tempConfigs = dropConfigs.Where(m => m.StartLevel <= level && m.EndLevel >= level && (level - m.StartLevel) % m.RateLevel == 0).ToList();

                InfiniteDropConfig config = RandomId(tempConfigs);

                if (config == null)
                {
                    rates.Add(0);
                    continue;
                }

                if (!dict.ContainsKey(config.Id))
                {
                    dict[config.Id] = 0;
                }

                dict[config.Id]++;

                if (dict[config.Id] >= config.Max)
                {
                    dropConfigs.Remove(config); //掉落上限的，去掉
                }

                rates.Add(config.DropId);
            }

            return rates;
        }

        private InfiniteDropConfig RandomId(List<InfiniteDropConfig> dropConfigs)
        {
            int total = dropConfigs.Select(m => m.Rate).Sum();
            int rd = RandomHelper.RandomNumber(1, total + 1);

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

        //private int RandomDropId(List<InfiniteDropConfig> dropConfigs)
        //{
        //    int total = dropConfigs.Select(m => m.Rate).Sum();
        //    int rd = RandomHelper.RandomNumber(1, total + 1);

        //    int endRate = 0;
        //    for (int i = 0; i < dropConfigs.Count; i++)
        //    {
        //        endRate += dropConfigs[i].Rate;

        //        if (rd <= endRate)
        //        {
        //            return dropConfigs[i].DropId;
        //        }
        //    }

        //    return -1;
        //}

        //private List<InfiniteDropConfig> GetLevelList(long level, List<int> excludeList)
        //{
        //    List<InfiniteDropConfig> configs = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level && (level - m.StartLevel) % m.RateLevel == 0).ToList();

        //    List<InfiniteDropConfig> list = new List<InfiniteDropConfig>();

        //    foreach (InfiniteDropConfig config in configs)
        //    {
        //        int total = excludeList.Where(m => m == config.DropId).Count();

        //        if (config.DropId >= 180001 && config.DropId <= 180100) //神器
        //        {
        //            ArtifactConfig artifactConfig = ArtifactConfigCategory.Instance.GetByItemId(config.DropId);
        //            int atLevel = GameProcessor.Inst.User.GetArtifactLevel(artifactConfig.Id);

        //            if (total + atLevel >= artifactConfig.MaxCount)
        //            {
        //                continue;
        //            }
        //        }

        //        if (config.Max > total)
        //        {
        //            list.Add(config);
        //        }
        //    }
        //    return list;
        //}
    }

}
