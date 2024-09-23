using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillSuitConfigCategory
    {

    }

    public class SkillSuitHelper
    {
        public static SkillSuitConfig RandomSuit(int seed, int skillId, int type)
        {
            List<SkillSuitConfig> list = SkillSuitConfigCategory.Instance.GetAll().Where(m => m.Value.SkillId == skillId && m.Value.Type == type).Select(m => m.Value).ToList();

            if (list.Count == 1)
            {
                return list[0];
            }

            int maxRate = list.Select(m => m.BuildRate).Sum();
            int rd = RandomHelper.RandomNumber(seed, 1, maxRate + 1);

            int tempRate = 0;
            for (int i = 0; i < list.Count; i++)
            {
                tempRate += list[i].BuildRate;

                if (rd <= tempRate)
                {
                    return list[i];
                }
            }

            return null;
        }

        public static List<SkillSuit> GetAllSuit(int skillId, int suitCount)
        {
            return GetAllSuit(skillId, suitCount, null);
        }

        public static List<SkillSuit> GetAllSuit(int skillId, int suitCount, int[] excludeList)
        {
            List<SkillSuit> suitList = new List<SkillSuit>();

            List<SkillSuitConfig> suitConfigs = SkillSuitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == skillId && m.Type == 1).OrderBy(m => m.Id).ToList();

            if (excludeList != null)
            {
                suitConfigs = suitConfigs.Where(m => !excludeList.Contains(m.Id)).ToList();
            }

            foreach (SkillSuitConfig config in suitConfigs)
            {
                SkillSuit suit = new SkillSuit(config.Id);
                if (suitList.Count < suitCount)
                {
                    suitList.Add(suit);
                }
            }

            return suitList;
        }
    }
}
