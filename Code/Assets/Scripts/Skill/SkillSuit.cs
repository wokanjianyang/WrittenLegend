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
        public int CritRate { get; } //������
        public int CritDamage { get; } //��������
        public int DamageIncrea { get; } //�˺��ӳ�

        public SkillSuit(int suitId)
        {
            this.SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(suitId);

            this.Damage = SkillSuitConfig.Damage;
            this.Percent = SkillSuitConfig.Percent;
            this.Dis = SkillSuitConfig.Dis;
            this.EnemyMax = SkillSuitConfig.EnemyMax;
            this.CD = SkillSuitConfig.CD;

            this.CritRate = SkillSuitConfig.CritRate;
            this.CritDamage = SkillSuitConfig.CritDamage;
            this.DamageIncrea = SkillSuitConfig.DamageIncrea;
        }
    }
}
