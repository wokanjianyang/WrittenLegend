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
        public static ExclusiveItem Build(int configId)
        {
            ExclusiveConfig config = ExclusiveConfigCategory.Instance.Get(configId);

            int quality = RandomQuanlity();

            int role = RandomHelper.RandomNumber(1, 4);

            int runeId = 0;
            int suitId = 0;
            if (quality >= 3)
            {
                SkillRuneConfig runeConfig = SkillRuneHelper.RandomRune(role);
                runeId = runeConfig.Id;

                if (quality >= 4)
                {
                    suitId = SkillSuitHelper.RandomSuit(runeConfig.SkillId).Id;
                }
            }

            int dhId = 0;
            if (quality >= 5)
            {
                dhId = SkillDoubleHitConfigCategory.RandomConfig().Id;
            }

            ExclusiveItem item = new ExclusiveItem(configId, runeId, suitId, quality, dhId);

            item.Count = 1;
            return item;
        }

        private static int RandomQuanlity() {
            int[] rates = { 1, 5, 10, 18, 32 };

            int r = RandomHelper.RandomNumber(0, 32);

            for (int i = 0; i < rates.Length; i++){
                if (r < rates[i]) {
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