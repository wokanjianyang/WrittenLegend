using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class ShengxiaoConfigCategory
    {
        public Shengxiao Build(int configId, double qualityRate, int maxQuality, int seed)
        {
            ShengxiaoConfig config = this.Get(configId);

            int quality = RandomQuanlity(qualityRate, maxQuality);

            List<KeyValuePair<int, long>> list = AttrEntryConfigCategory.Instance.BuildShengxiao(config.Part, quality, seed);

            Shengxiao item = new Shengxiao(configId, quality);
            item.Init(list);

            item.Count = 1;

            return item;
        }

        private int RandomQuanlity(double realRate, int maxQuality)
        {
            int[] rates = { 1, 4, 10, 33, 250, 1000, 3000, 9000, 40000 };

            int r = RandomHelper.RandomNumber(0, rates[maxQuality - 1]);

            r = (int)(r / realRate);

            for (int i = 0; i < maxQuality; i++)
            {
                if (r < rates[i])
                {
                    return maxQuality - i;
                }
            }

            return 1;
        }

        public Shengxiao BuildByPack(int configId)
        {
            ShengxiaoConfig config = this.Get(configId);

            List<KeyValuePair<int, long>> list = AttrEntryConfigCategory.Instance.BuildMaxShengxiao(config.Part, 9);

            Shengxiao item = new Shengxiao(configId, 9);
            item.Init(list);

            item.Count = 1;

            return item;
        }
    }
}