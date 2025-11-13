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
            int[] rates = { 1, 5, 25, 50, 500, 1000, 3000, 10000, 30000 };

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

        public static Equip BuildByPack(int configId)
        {
            GiftPackEquipConfig config = GiftPackEquipConfigCategory.Instance.Get(configId);

            Equip item = new Equip(config.EquipId, config.RuneId, config.SuitId, config.Quality);

            List<KeyValuePair<int, long>> AttrEntryList = new List<KeyValuePair<int, long>>();

            for (int i = 0; i < config.AttrIdList.Length; i++)
            {
                int attrId = config.AttrIdList[i];
                AttrEntryConfig entryConfig = AttrEntryConfigCategory.Instance.GetRedConfig(attrId, config.Cycle);
                AttrEntryList.Add(new KeyValuePair<int, long>(attrId, entryConfig.MaxValue));
            }

            item.AttrEntryList = AttrEntryList;

            item.Count = 1;
            return item;
        }
    }
}