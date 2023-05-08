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
    }
}
