using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillRune
    {
        //生效数量
        public int AvailableQuantity { get; }

        public SkillRuneConfig SkillRuneConfig { get; }

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

        public SkillRune(int runeId, int quantity)
        {
            this.SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(runeId);
            this.AvailableQuantity = Mathf.Min(quantity, SkillRuneConfig.Max);

            this.Damage = SkillRuneConfig.Damage * AvailableQuantity;
            this.Percent = SkillRuneConfig.Percent * AvailableQuantity;
            this.Dis = SkillRuneConfig.Dis * AvailableQuantity;
            this.EnemyMax = SkillRuneConfig.EnemyMax * AvailableQuantity;
            this.Duration = SkillRuneConfig.Duration * AvailableQuantity;
            this.CD = SkillRuneConfig.CD * AvailableQuantity;
            this.Row = SkillRuneConfig.Row * AvailableQuantity;
            this.Column = SkillRuneConfig.Column * AvailableQuantity;

            this.IgnoreDef = SkillRuneConfig.IgnoreDef * AvailableQuantity;

            this.CritRate = SkillRuneConfig.CritRate * AvailableQuantity;
            this.CritDamage = SkillRuneConfig.CritDamage * AvailableQuantity;
            this.DamageIncrea = SkillRuneConfig.DamageIncrea * AvailableQuantity;

            this.AttrIncrea = SkillRuneConfig.AttrIncrea;
            this.FinalIncrea = SkillRuneConfig.FinalIncrea;
            this.InheritIncrea = SkillRuneConfig.InheritIncrea;

            this.EffectId = SkillRuneConfig.EffectId;
        }
    }
}
