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

        public int CritRate { get; } //��������

        public AttackGeometryType Area { get; }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList)
        {
            this.SkillData = skillData; //���ܱ���
            this.RuneList = runeList;  //����
            this.SuitList = suitList; //��װ
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

            this.Damage += skillData.SkillConfig.Damage + runeDamage + suitDamage;
            this.Percent += skillData.SkillConfig.Percent + runePercent + suitPercent;
            this.Dis += skillData.SkillConfig.Dis + runeDis + suitDis;
            this.EnemyMax += skillData.SkillConfig.EnemyMax + runeEnemyMax + suitEnemyMax;
            this.CD += Mathf.Max(skillData.SkillConfig.CD - runeCD - suitCD, 0);
            this.Duration = skillData.SkillConfig.Duration + runeDuration + suitDuration;

            this.CritRate = 0;

            //ʩ����Χ
            this.Area = EnumHelper.FromString<AttackGeometryType>(skillData.SkillConfig.Area);
        }
    }
}
