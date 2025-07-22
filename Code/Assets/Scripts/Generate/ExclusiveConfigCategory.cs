using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class ExclusiveConfigCategory
    {
        public List<ExclusiveConfig> GetByCycle(int cycle)
        {
            return this.list.Where(m => m.Cycle == cycle).ToList();
        }

    }

    public class ExclusiveHelper
    {

        public static ExclusiveItem Build(int configId, int qualityRate, int seed)
        {
            if (configId <= 6)
            {
                return Build(configId, seed);
            }

            //AppHelper.TempRecord++;
            //Debug.Log("TempRecord:" + AppHelper.TempRecord);

            ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(configId);

            double realRate = MathHelper.ConvertionDropRate(qualityRate, 50);
            int quality = RandomNewQuality(realRate) + config.Cycle - 2;

            int runeId = 0;
            int suitId = 0;

            SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.GetExclusiveRune(quality, seed);

            if (runeConfig != null)
            {
                runeId = runeConfig.Id;

                if (quality == 8)
                {
                    suitId = SkillSuitConfigCategory.Instance.GetSuitIdBySkillLayer(runeConfig.SkillLayer);
                }
                else
                {
                    suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId, runeConfig.Type).Id;
                }
            }

            //if (quality == 7)
            //{
            //    AppHelper.TempRecord1++;
            //    Debug.Log("TempRecord Golden:" + AppHelper.TempRecord1);
            //}

            ExclusiveItem item = new ExclusiveItem(configId, runeId, suitId, quality, 0);
            if (seed < 0)
            {
                seed = AppHelper.InitSeed();
            }
            item.Init(seed);

            item.Count = 1;
            return item;
        }


        public static ExclusiveItem Build(int configId, int seed)
        {
            //if (seed < 0)
            //{
            //    seed = AppHelper.InitSeed();
            //}

            ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(configId);

            int quality = 0;
            int runeId = config.RuneId;
            int suitId = config.SuitId;

            if (quality <= 0)
            {
                quality = RandomQuanlity();
            }

            if (quality >= 3)
            {
                int role = RandomHelper.RandomNumber(1, 4);

                SkillRuneConfig runeConfig;
                if (runeId <= 0)
                {
                    runeConfig = SkillRuneConfigCategory.Instance.RandomRune(seed, -1, role, 0, quality, 0);
                    runeId = runeConfig.Id;
                }
                else
                {
                    runeConfig = SkillRuneConfigCategory.Instance.Get(runeId);
                }

                if (suitId <= 0 && quality >= 4)
                {
                    suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId, runeConfig.Type).Id;
                }
            }

            int dhId = 0;
            if (quality >= 5)
            {
                dhId = SkillDoubleHitConfigCategory.RandomConfig().Id;
            }

            ExclusiveItem item = new ExclusiveItem(configId, runeId, suitId, quality, dhId);
            if (seed < 0)
            {
                seed = AppHelper.InitSeed();
            }
            item.Init(seed);

            item.Count = 1;
            return item;
        }

        public static ExclusiveItem BuildByPack(int configId)
        {
            GiftPackExclusiveConfig config = GiftPackExclusiveConfigCategory.Instance.Get(configId);

            ExclusiveItem item = new ExclusiveItem(config.ExclusiveId, config.RuneId, config.SuitId, config.Quality, config.DoubeId);

            item.Count = 1;
            return item;
        }

        private static int RandomQuanlity()
        {
            int[] rates = { 1, 5, 10, 18, 32 };

            int r = RandomHelper.RandomNumber(0, 32);

            for (int i = 0; i < rates.Length; i++)
            {
                if (r < rates[i])
                {
                    return 5 - i;
                }
            }

            return 1;
        }


        private static int RandomNewQuality(double qualityRate)
        {
            int[] rates = { 1, 10, 100, 1000, 3000, 10000, 50000 };

            //int[] rates = { 1, 10, 200, 300, 400, 500, 600 };

            int r = RandomHelper.RandomNumber(0, rates[6]);

            r = (int)(r / qualityRate);

            for (int i = 0; i < rates.Length; i++)
            {
                if (r < rates[i])
                {
                    return 7 - i;
                }
            }

            return 1;
        }
    }

    public class ExclusiveSuitItem
    {
        public ExclusiveSuitItem(int id, string name, bool active)
        {
            this.Id = id;
            this.Name = name;
            this.Active = active;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public bool Active { get; set; }
    }

    public class ExclusiveSuit
    {
        public ExclusiveSuit(int cycle)
        {
            this.SuitConfig = ExclusiveSuitConfigCategory.Instance.Get(cycle);
        }

        public ExclusiveSuitConfig SuitConfig { get; set; }

        public bool Active { get; set; } = false;

        public int ActiveCount { get; set; } = 0;

        public List<ExclusiveSuitItem> ItemList = new List<ExclusiveSuitItem>();
    }
}