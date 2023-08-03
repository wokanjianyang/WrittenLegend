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

        public int Row { get; }
        public int Column { get; }

        public int Duration { get; }

        public int IgnoreDef { get; }  //无视防御

        public int CritRate { get; } //暴击率
        public int CritDamage { get; } //暴击倍率
        public int DamageIncrea { get; } //伤害加成
        public int AttrIncrea { get; } //攻击加成

        public int FinalIncrea { get; } //最终伤害加成

        public Dictionary<int, EffectData> EffectIdList { get; } = new Dictionary<int, EffectData>(); //特殊效果 

        public AttackGeometryType Area { get; }

        public AttackCastType CastType { get; }

        public SkillPanel(SkillData skillData, List<SkillRune> runeList, List<SkillSuit> suitList)
        {
            this.SkillData = skillData; //技能本身
            this.RuneList = runeList;  //词条
            this.SuitList = suitList; //套装
            this.SkillId = skillData.SkillId;

            //套装的特效
            foreach (SkillSuit suit in suitList)
            {
                if (suit.EffectId > 0 && !EffectIdList.ContainsKey(suit.EffectId))
                {
                    int fromId = (int)AttributeFrom.Skill * 100000 + SkillId + suit.EffectId;
                    EffectIdList[suit.EffectId] = new EffectData(suit.EffectId, fromId, suit.Percent, suit.Damage, suit.Duration, 1); //暂且设置只能叠加一层
                }
            }

            int levelPercent = skillData.Level ;
            long levelDamage = skillData.Level * 1000;

            long runeDamage = runeList.Select(m => m.Damage).Sum();
            long suitDamage = suitList.Select(m => m.Damage).Sum();

            int runePercent = runeList.Select(m => m.Percent).Sum();
            int suitPercent = suitList.Select(m => m.Percent).Sum();

            int runeIgnoreDef = runeList.Select(m => m.IgnoreDef).Sum();
            int suitIgnoreDef = suitList.Select(m => m.Percent).Sum();

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

            int runeRow = runeList.Select(m => m.Row).Sum();
            int suitRow = suitList.Select(m => m.Row).Sum();

            int runeColumn = runeList.Select(m => m.Column).Sum();
            int suitColumn = suitList.Select(m => m.Column).Sum();

            this.Damage += skillData.SkillConfig.Damage + runeDamage + suitDamage + levelDamage;
            this.Percent += skillData.SkillConfig.Percent + runePercent + suitPercent + levelPercent;

            this.IgnoreDef += skillData.SkillConfig.IgnoreDef + runeIgnoreDef + suitIgnoreDef;
            this.Dis += skillData.SkillConfig.Dis + runeDis + suitDis;
            this.EnemyMax += skillData.SkillConfig.EnemyMax + runeEnemyMax + suitEnemyMax;
            this.CD += Mathf.Max(skillData.SkillConfig.CD - runeCD - suitCD, 0);
            this.Duration = skillData.SkillConfig.Duration + runeDuration + suitDuration;

            this.Row = skillData.SkillConfig.Row + runeRow + suitRow;
            this.Column = skillData.SkillConfig.Column + runeColumn + suitColumn;

            this.CritRate = skillData.SkillConfig.CritRate + runeCritRate + suitCritRate;
            this.CritDamage = skillData.SkillConfig.CritRate + runeCritDamage + suitCritDamage;
            this.DamageIncrea = skillData.SkillConfig.CritRate + runeDamageIncrea + suitDamageIncrea;

            this.AttrIncrea = 0 + runeAttrIncrea + suitAttrIncrea;
            this.FinalIncrea = 0 + runeFinalIncrea + suitFinalIncrea;

            //施法范围
            this.Area = EnumHelper.FromString<AttackGeometryType>(skillData.SkillConfig.Area);
            this.CastType = (AttackCastType)skillData.SkillConfig.CastType;

            //技能的特效
            string[] skilEffectList = skillData.SkillConfig.EffectList;
            if (skilEffectList != null && skilEffectList.Length > 0)
            {
                foreach (string skillEffect in skilEffectList)
                {
                    int[] effectParams = StringHelper.ConvertSkillParams(skillEffect);
                    int effectId = effectParams[0];
                    int duration = effectParams[1] >= 0 ? effectParams[1] : Duration; //如果为-1,则使用技能的配置
                    int max = effectParams[2] >= 0 ? effectParams[2] : EnemyMax;
                    int percent = effectParams[3];

                    if (effectId > 0 && !EffectIdList.ContainsKey(effectId)) //不能叠加特效
                    {
                        int fromId = (int)AttributeFrom.Skill * 100000 + SkillId + effectId;

                        EffectIdList[effectId] = new EffectData(effectId, fromId, percent, 0, duration, max);
                    }
                }
            }
            
            //TEST skill
            //this.CD = 0;
            //this.Row = 2;
            //this.Column = 2;
        }
    }
}
