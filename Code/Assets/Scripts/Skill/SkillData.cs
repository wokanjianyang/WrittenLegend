using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillData
    {
        public SkillData(int skillId) {
            this.SkillId = skillId;

            SkillConfig = SkillConfigCategory.Instance.Get(skillId);

            this.Name = SkillConfig.Name;
            this.Des = SkillConfig.Des;
            this.CD = SkillConfig.CD;
        }

        //����״̬
        public SkillStatus Status { get; set; } 

        public int SkillId { get; set; }
        public string Name { get; set; }

        public string Des { get; set; }

        public int CD { get; set; }

        public long Exp { get; set; }

        public long UpExp { get; set; }

        public SkillConfig SkillConfig { get; set; }

        public void AddExp(long exp)
        {
            this.Exp += exp;
            while (this.Exp >= this.GetUpExp())
            {
                var upExp = this.GetUpExp();
                this.Level++;
                this.Exp -= upExp;
            }
        }

        public long GetUpExp()
        {
            return this.Level * 100;
        }
        //----------------


        public int Type { get; set; }

        public int Priority { get; set; }

        /// <summary>
        /// ʩ������ 1 ���� 2����
        /// </summary>
        public int CastType { get; set; }

        /// <summary>
        /// ְҵ 1 սʿ 2 ��ʦ 3 ��ʿ
        /// </summary>
        public int Role { get; set; }

        public int Level { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int Dis { get; set; }

        /// <summary>
        /// �������� self enemy
        /// </summary>
        public SkillCenter Center { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        public AttackGeometryType Area { get; set; }

        /// <summary>
        /// ��󹥻���������
        /// </summary>
        public int EnemyMax { get; set; }

        /// <summary>
        /// �˺�����
        /// </summary>
        public int Percent { get; set; }

        /// <summary>
        /// �̶��˺�
        /// </summary>
        public int Damage { get; set; }
    }

    public enum SkillType
    {
        Attack = 1,
        Valet = 2,
    }

    public enum SkillStatus
    {
        /// <summary>
        /// δѧϰ
        /// </summary>
        Normal = 0,
        /// <summary>
        /// ��ѧϰ
        /// </summary>
        Learn = 1,
        /// <summary>
        /// ��װ��
        /// </summary>
        Equip = 2,
    }
}
