using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class SkillRuneConfigCategory
    {

    }

    public class SkillRuneHelper
    {
        public static SkillRuneConfig RandomRune(int role, int type, int quality, int level, int maxRuneLevel)
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

            int end = RuneRate.Length - 1;
            int start = 0;
            if (level >= 700 && quality >= 5)
            { //700级以下，没有9级词条
                start = 1;
            }
            if (maxRuneLevel > 0)
            {
                start = end - maxRuneLevel;
            }

            int index = RandomHelper.RandomNumber(RuneRate[start], RuneRate[end]);

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
