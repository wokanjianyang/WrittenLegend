using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class SkillPanel
    {
        public SkillData SkillData { get; set; }

        public List<SkillRune> RuneList { get; }

        public List<SkillSuit> SuitList { get; }

        public int SkillId { get; }

        public long Damage { get; }
        public int Percent { get; }
        public int Dis { get; }
        public int EnemyMax { get; }
        public int CD { get; }

        public int Duration { get; }

        public int CritRate { get; } //暴击率
        public int CritDamage { get; } //暴击倍率
        public int DamageIncrea { get; } //伤害加成
        public int AttrIncrea { get; } //攻击加成

        public int FinalIncrea { get; } //最终伤害加成


        public AttackGeometryType Area { get; }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList)
        {
            this.SkillData = skillData; //技能本身
            this.RuneList = runeList;  //词条
            this.SuitList = suitList; //套装
            this.SkillId = skillData.SkillId;

            long runeDamage = runeList.Select(m => m.Damage).Sum();
            long suitDamage = suitList.Select(m => m.Damage).Sum();

            int runePercent = runeList.Select(m => m.Percent).Sum();
            int suitPercent = suitList.Select(m => m.Percent).Sum();

            int runeDis = runeList.Select(m => m.Dis).Sum();
            int suitDis = suitList.Select(m => m.Dis).Sum();

            int runeEnemyMax = runeList.Select(m => m.EnemyMax).Sum();
            int suitEnemyMax = suitList.Select(m => m.EnemyMax).Sum();

            int runeCD = runeList.Select(m => m.CD).Sum();
            int suitCD = suitList.Select(m => m.CD).Sum();

            int runeDuration = runeList.Select(m => m.CD).Sum();
            int suitDuration = suitList.Select(m => m.CD).Sum();

            int runeCritRate = runeList.Select(m => m.CritRate).Sum();
            int suitCritRate = suitList.Select(m => m.CritRate).Sum();

            int runeCritDamage = runeList.Select(m => m.CritDamage).Sum();
            int suitCritDamage = suitList.Select(m => m.CritDamage).Sum();

            int runeDamageIncrea = runeList.Select(m => m.DamageIncrea).Sum();
            int suitDamageIncrea = suitList.Select(m => m.DamageIncrea).Sum();

            int runeAttrIncrea = runeList.Select(m => m.AttrIncrea).Sum();
            int suitAttrIncrea = suitList.Select(m => m.AttrIncrea).Sum();

            int runeFinalIncrea = runeList.Select(m => m.FinalIncrea).Sum();
            int suitFinalIncrea = suitList.Select(m => m.FinalIncrea).Sum();

            this.Damage += skillData.SkillConfig.Damage + runeDamage + suitDamage;
            this.Percent += skillData.SkillConfig.Percent + runePercent + suitPercent;
            this.Dis += skillData.SkillConfig.Dis + runeDis + suitDis;
            this.EnemyMax += skillData.SkillConfig.EnemyMax + runeEnemyMax + suitEnemyMax;
            this.CD += Mathf.Max(skillData.SkillConfig.CD - runeCD - suitCD, 0);
            this.Duration = skillData.SkillConfig.Duration + runeDuration + suitDuration;

            this.CritRate = skillData.SkillConfig.CritRate + runeCritRate+suitCritRate;
            this.CritDamage = skillData.SkillConfig.CritRate + runeCritDamage + suitCritDamage;
            this.DamageIncrea = skillData.SkillConfig.CritRate + runeDamageIncrea + suitDamageIncrea;

            this.AttrIncrea = 0 + runeAttrIncrea + suitAttrIncrea;
            this.FinalIncrea = 0 + runeFinalIncrea + suitFinalIncrea;

            //施法范围
            this.Area = EnumHelper.FromString<AttackGeometryType>(skillData.SkillConfig.Area);
        }
    }
}
