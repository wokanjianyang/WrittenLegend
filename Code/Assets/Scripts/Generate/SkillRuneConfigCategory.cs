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
                RuneRate = ConfigHelper.RuneRate;

                if (maxRuneLevel > 0)
                {
                    RuneRate = ConfigHelper.RuneRate0;
                }
                else if (quality >= 5)
                {
                    if (level <= 300)
                    {
                        RuneRate = ConfigHelper.RuneRate1;
                    }
                    else if (level <= 650)
                    {
                        RuneRate = ConfigHelper.RuneRate2;
                    }
                    else
                    {
                        RuneRate = ConfigHelper.RuneRate3;
                    }
                }
            }
            else
            {
                RuneRate = ConfigHelper.RuneRate2;
            }

            int index = RandomHelper.RandomNumber(0, RuneRate[RuneRate.Length - 1]);
            if (index == 0)
            {
                Debug.Log("index:" + index);
            }

            for (int i = 0; i < RuneRate.Length; i++)
            {
                if (index < RuneRate[i])
                {
                    skillId = skillId + (RuneRate.Length - i);

                    if (index == 0 && RuneRate.Length == 9)
                    {
                        Debug.Log("Rune3:" + skillId);
                    }

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
