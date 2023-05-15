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
        public int EnemyMax { get; }
        public int CD { get; }

        public int IgnoreDef { get; } //无视防御
        public int CritRate { get; } //暴击率
        public int CritDamage { get; } //暴击倍率
        public int DamageIncrea { get; } //伤害加成
        public int AttrIncrea { get; } //攻击加成
        public int FinalIncrea { get; } //最终伤害加成

        public SkillSuit(int suitId)
        {
            this.SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(suitId);

            this.Damage = SkillSuitConfig.Damage;
            this.Percent = SkillSuitConfig.Percent;
            this.Dis = SkillSuitConfig.Dis;
            this.EnemyMax = SkillSuitConfig.EnemyMax;
            this.CD = SkillSuitConfig.CD;

            this.IgnoreDef = SkillSuitConfig.IgnoreDef;
            this.CritRate = SkillSuitConfig.CritRate;
            this.CritDamage = SkillSuitConfig.CritDamage;
            this.DamageIncrea = SkillSuitConfig.DamageIncrea;

            this.AttrIncrea = SkillSuitConfig.AttrIncrea;
            this.FinalIncrea = SkillSuitConfig.FinalIncrea;
        }
    }
}
