using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillSuit
    {
        public SkillSuitConfig SkillSuitConfig { get; }

        public long Damage { get; }
        public int Percent { get; }
        public int Dis { get; }

        public int Duration { get; }
        public int EnemyMax { get; }
        public int CD { get; }
        public int Row { get; }
        public int Column { get; }
        public int IgnoreDef { get; } //无视防御
        public int CritRate { get; } //暴击率
        public int CritDamage { get; } //暴击倍率
        public int DamageIncrea { get; } //伤害加成
        public int AttrIncrea { get; } //攻击加成
        public int FinalIncrea { get; } //最终伤害加成

        public int InheritIncrea { get; }
        public int EffectId { get; } //

        public SkillSuit(int suitId)
        {
            this.SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(suitId);

            this.Damage = SkillSuitConfig.Damage;
            this.Percent = SkillSuitConfig.Percent;
            this.Dis = SkillSuitConfig.Dis;
            this.Duration = SkillSuitConfig.Duration;
            this.EnemyMax = SkillSuitConfig.EnemyMax;
            this.CD = SkillSuitConfig.CD;
            this.Row = SkillSuitConfig.Row;
            this.Column = SkillSuitConfig.Column;

            this.IgnoreDef = SkillSuitConfig.IgnoreDef;
            this.CritRate = SkillSuitConfig.CritRate;
            this.CritDamage = SkillSuitConfig.CritDamage;
            this.DamageIncrea = SkillSuitConfig.DamageIncrea;

            this.AttrIncrea = SkillSuitConfig.AttrIncrea;
            this.FinalIncrea = SkillSuitConfig.FinalIncrea;
            this.InheritIncrea = SkillSuitConfig.InheritIncrea;

            this.EffectId = SkillSuitConfig.EffectId;
        }
    }
}
