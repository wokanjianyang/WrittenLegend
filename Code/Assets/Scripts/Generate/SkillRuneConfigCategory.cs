using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillRuneConfigCategory
    {

    }

    public class SkillRuneHelper
    {
        public static SkillRuneConfig RandomRune(int role, int type)
        {
            int skillId = role * 1000;

            int[] RuneRate = type == 1 ? ConfigHelper.RuneRate : ConfigHelper.RuneRate1;

            int index = RandomHelper.RandomNumber(0, RuneRate[RuneRate.Length - 1]);

            for (int i = 0; i < RuneRate.Length; i++)
            {
                if (index < RuneRate[i])
                {
                    skillId = skillId + (7 - i);
                    break;
                }
            }

            List<SkillRuneConfig> list = SkillRuneConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == skillId).ToList();

            index = RandomHelper.RandomNumber(0, list.Count);

            return list[index];
        }
    }
}
