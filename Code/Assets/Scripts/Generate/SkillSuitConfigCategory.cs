using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillSuitConfigCategory
    {

    }

    public class SkillSuitHelper {
        public static SkillSuitConfig RandomSuit(int skillId)
        {
            List<SkillSuitConfig> list = SkillSuitConfigCategory.Instance.GetAll().Where(m => m.Value.SkillId == skillId).Select(m => m.Value).ToList();
            int index = RandomHelper.RandomNumber(0, list.Count);
            return list[index];
        }

        public static List<SkillSuit> GetAllSuit(int skillId)
        {
            List<SkillSuit> suitList = new List<SkillSuit>();

            List<SkillSuitConfig> suitConfigs = SkillSuitConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == skillId).OrderBy(m => m.Id).ToList();
            foreach (SkillSuitConfig config in suitConfigs)
            {
                SkillSuit suit = new SkillSuit(config.Id);
                suitList.Add(suit);
            }

            return suitList;
        }
    }
}
