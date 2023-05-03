using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillRune
    {
        //��Ч����
        public int AvailableQuantity { get; }

        public SkillRuneConfig SkillRuneConfig { get; }

        public long Damage { get; }
        public int Percent { get; }
        public int Dis { get; }
        public int EnemyMax { get; }
        public int CD { get; }

        public int CritRate { get; } //������
        public int CritDamage { get; } //��������
        public int DamageIncrea { get; } //�˺��ӳ�

        public SkillRune(int runeId, int quantity)
        {
            this.SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(runeId);
            this.AvailableQuantity = Mathf.Min(quantity, SkillRuneConfig.Max);

            this.Damage = SkillRuneConfig.Damage * AvailableQuantity;
            this.Percent = SkillRuneConfig.Percent * AvailableQuantity;
            this.Dis = SkillRuneConfig.Dis * AvailableQuantity;
            this.EnemyMax = SkillRuneConfig.EnemyMax * AvailableQuantity;
            this.CD = SkillRuneConfig.CD * AvailableQuantity;

            this.CritRate = SkillRuneConfig.CritRate * AvailableQuantity;
            this.CritDamage = SkillRuneConfig.CritDamage * AvailableQuantity;
            this.DamageIncrea = SkillRuneConfig.DamageIncrea * AvailableQuantity;
        }
    }
}
