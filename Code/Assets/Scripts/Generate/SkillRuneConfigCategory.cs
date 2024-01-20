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
        public static SkillRuneConfig RandomRune(int role, int type, int quality,int level)
        {
            int skillId = role * 1000;

            int[] RuneRate;
            if (type == 1)
            {
                RuneRate = quality < 5 ? ConfigHelper.RuneRate : ConfigHelper.RuneRate1;
            }
            else
            {
                RuneRate = ConfigHelper.RuneRate2;
            }

            int runeMax = RuneRate.Length - 1;
            if (level < 700 && runeMax == 8)
            { //700级以下，没有9级词条
                runeMax = 7;
            }

            int index = RandomHelper.RandomNumber(0, RuneRate[runeMax]);

            for (int i = 0; i < RuneRate.Length; i++)
            {
                if (index < RuneRate[i])
                {
                    skillId = skillId + (RuneRate.Length - i);
                    break;
                }
            }

            List<SkillRuneConfig> list = SkillRuneConfigCategory.Instance.GetAll().Select(m => m.Value)
                .Where(m => m.SkillId == skillId && m.Type == 1).ToList();

            index = RandomHelper.RandomNumber(0, list.Count);

            return list[index];
        }
    }
}
