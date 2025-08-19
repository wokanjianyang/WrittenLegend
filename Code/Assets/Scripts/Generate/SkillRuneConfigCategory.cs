using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class SkillRuneConfigCategory
    {
        public SkillRuneConfig Random7(int type, int role)
        {
            List<SkillRuneConfig> list = this.list.Where(m => m.Type == type && m.Role == role).ToList(); //选择金色词条

            int maxRate = list.Select(m => m.EquipRate).Sum();
            int rd = RandomHelper.RandomNumber(1, maxRate + 1);

            int tempRate = 0;
            for (int i = 0; i < list.Count; i++)
            {
                tempRate += list[i].EquipRate;

                if (rd <= tempRate)
                {
                    return list[i];
                }
            }

            return null;
        }

        public SkillRuneConfig RandomRune(int seed, int indexSeed, int role, int type, int quality, int level)
        {
            int skillId = role * 1000;

            int[] RuneRate;
            if (type == 1)
            {
                RuneRate = ConfigHelper.RuneRate;

                if (quality == 7)
                {
                    return Random7(2, role);
                }
                else if (quality == 8)
                {
                    return Random7(3, 0);
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
                RuneRate = ConfigHelper.RuneRate99;
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

            List<SkillRuneConfig> list = this.list.Where(m => m.SkillId == skillId && m.Type == 1).ToList();

            index = RandomHelper.RandomNumber(indexSeed, 0, list.Count);

            return list[index];
        }

        public List<SkillRune> GetAllRune(int skillId, int runeCount)
        {
            List<SkillRune> runeList = new List<SkillRune>();

            List<SkillRuneConfig> runeConfigs = this.list.Where(m => m.SkillId == skillId && m.Type == 1).OrderBy(m => m.Id).ToList();

            foreach (SkillRuneConfig config in runeConfigs)
            {
                SkillRune skillRune = new SkillRune(config.Id, runeCount);
                runeList.Add(skillRune);
            }
            return runeList;
        }

        public List<SkillRuneConfig> GetSkillAllConfigs(int skillId, int skillLayer)
        {
            return this.list.Where(m => (m.SkillId == skillId) || (m.SkillLayer == skillLayer)).ToList();

        }

        public SkillRuneConfig GetExclusiveRune(int quality, int seed)
        {
            List<SkillRuneConfig> tempList = this.list.Where(m => m.StartQuality <= quality && quality <= m.EndQuality).ToList();

            if (tempList.Count == 1)
            {
                return tempList[0];
            }

            int maxRate = tempList.Select(m => m.ExclusiveRate).Sum();
            int rd = RandomHelper.RandomNumber(seed, 1, maxRate + 1);

            int tempRate = 0;
            for (int i = 0; i < tempList.Count; i++)
            {
                tempRate += tempList[i].ExclusiveRate;

                if (rd <= tempRate)
                {
                    return tempList[i];
                }
            }

            return null;
        }

        public SkillRuneConfig GeEquipRuneCycle5(int quality, int role, int seed)
        {
            List<SkillRuneConfig> tempList = this.list.Where(m => (m.Role == role) && m.Type == 5 && m.StartQuality <= quality && quality <= m.EndQuality).ToList();

            if (tempList.Count == 0)
            {
                //如果品质不匹配，则使用低级的词条
                tempList = this.list.Where(m => (m.Role == role) && m.Type == 1).ToList();
            }

            if (tempList.Count == 1)
            {
                return tempList[0];
            }

            int maxRate = tempList.Select(m => m.EquipRate).Sum();
            int rd = RandomHelper.RandomNumber(seed, 1, maxRate + 1);

            int tempRate = 0;
            for (int i = 0; i < tempList.Count; i++)
            {
                tempRate += tempList[i].EquipRate;

                if (rd <= tempRate)
                {
                    return tempList[i];
                }
            }

            return null;
        }
    }


}
