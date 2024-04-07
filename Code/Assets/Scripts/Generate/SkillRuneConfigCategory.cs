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
        public static SkillRuneConfig RandomRune(int seed,int indexSeed, int role, int type, int quality, int level)
        {
            int skillId = role * 1000;

            int[] RuneRate;
            if (type == 1)
            {
                RuneRate = ConfigHelper.RuneRate;
                if (quality >= 5)
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

            int mx = RuneRate[RuneRate.Length - 1];
            int index = RandomHelper.RandomNumber(seed, 0, mx);

            for (int i = 0; i < RuneRate.Length; i++)
            {
                if (index < RuneRate[i])
                {
                    skillId = skillId + (RuneRate.Length - i);

                    break;
                }
            }

            //seed = AppHelper.RefreshSeed(seed);

            List<SkillRuneConfig> list = SkillRuneConfigCategory.Instance.GetAll().Select(m => m.Value)
                .Where(m => m.SkillId == skillId && m.Type == 1).ToList();

            index = RandomHelper.RandomNumber(indexSeed, 0, list.Count);

            return list[index];
        }


        public static List<SkillRune> GetAllRune(int skillId, int runeCount)
        {
            List<SkillRune> runeList = new List<SkillRune>();

            List<SkillRuneConfig> runeConfigs = SkillRuneConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.SkillId == skillId).OrderBy(m => m.Id).ToList();

            foreach (SkillRuneConfig config in runeConfigs)
            {
                SkillRune skillRune = new SkillRune(config.Id, runeCount);
                runeList.Add(skillRune);
            }
            return runeList;
        }
    }
}
