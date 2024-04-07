using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ExclusiveConfigCategory
    {

    }

    public class ExclusiveHelper
    {
        public static ExclusiveItem Build(int configId, int seed)
        {
            //if (seed < 0)
            //{
            //    seed = AppHelper.InitSeed();
            //}

            ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(configId);

            int quality = config.Quality;
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
                    runeConfig = SkillRuneHelper.RandomRune(seed, -1, role, quality, 0, 0);
                    runeId = runeConfig.Id;
                }
                else
                {
                    runeConfig = SkillRuneConfigCategory.Instance.Get(runeId);
                }

                if (suitId <= 0 && quality >= 4)
                {
                    suitId = SkillSuitHelper.RandomSuit(seed, runeConfig.SkillId).Id;
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
        public bool Active { get; set; } = false;

        public int ActiveCount { get; set; } = 0;

        public List<ExclusiveSuitItem> ItemList = new List<ExclusiveSuitItem>();
    }
}