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
            ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(configId);

            if (config.Cycle == 1)
            {
                return Build(configId, seed);
            }
            else if (config.Cycle == 2)
            {
                //AppHelper.TestExclusive2++;
                //Debug.Log("TestExclusive2:" + AppHelper.TestExclusive2);

                return BuildCycle2(configId, qualityRate, seed);
            }
            else if (config.Cycle == 3)
            {
                //AppHelper.TestExclusive3++;
                //Debug.Log("TestExclusive3:" + AppHelper.TestExclusive3);

                return BuildCycle3(configId, qualityRate, seed);
            }

            return null;
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

        public static ExclusiveItem BuildCycle2(int configId, int qualityRate, int seed)
        {
            int quality = RandomNewQualityCycle2(qualityRate);

            int runeId = 0;
            int suitId = 0;

            SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.GetExclusiveRune(quality, seed);

            if (runeConfig != null)
            {
                runeId = runeConfig.Id;
                suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId, runeConfig.Type).Id;
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
        public static ExclusiveItem BuildCycle3(int configId, int qualityRate, int seed)
        {
            //if (seed < 0)

            ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(configId);

            int quality = RandomNewQualityCycle3(qualityRate);

            int runeId = 0;
            int suitId = 0;

            SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.GetExclusiveRune(quality, seed);

            if (runeConfig != null)
            {
                runeId = runeConfig.Id;

                if (runeConfig.SkillId == 0)
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


        private static int RandomNewQualityCycle2(double qualityRate)
        {
            int[] rates = { 1, 10, 100, 1000, 10000, 50000, 250000 };

            //int[] rates = { 1, 10, 200, 300, 400, 500, 600 };
            int start = 0;

            int r = RandomHelper.RandomNumber(0, rates[6]);

            r = (int)(r / qualityRate);

            for (int i = 0; i < rates.Length; i++)
            {
                if (r < rates[i])
                {
                    if (i == 0)
                    {
                        //防止SL，给最高品质-1
                        start = AppHelper.GetLossQuality();
                    }

                    return 7 - i - start;
                }
            }

            return 1;
        }

        private static int RandomNewQualityCycle3(double qualityRate)
        {
            int[] rates = { 1, 4, 16, 100, 1000, 10000, 100000, 600000 };

            //int[] rates = { 1, 10, 200, 300, 400, 500, 600, 600 };
            int start = 0;

            int r = RandomHelper.RandomNumber(0, rates[7]);

            r = (int)(r / qualityRate);

            for (int i = 0; i < rates.Length; i++)
            {
                if (r < rates[i])
                {
                    if (i == 0)
                    {
                        //防止SL，给最高品质-1
                        start = AppHelper.GetLossQuality();
                    }

                    return 8 - i - start;
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