using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillData
    {
        public int SkillId { get; set; }
        public long Exp { get; set; }
        public int Level { get; set; }

        //����״̬
        public SkillStatus Status { get; set; }

        //װ��λ��
        public int Position { get; set; }

        [JsonIgnore]
        public SkillConfig SkillConfig { get; set; }

        public SkillData(int skillId,int position) {
            this.SkillId = skillId;
            this.Position = position;
            SkillConfig = SkillConfigCategory.Instance.Get(skillId);
        }

        public void AddExp(long exp)
        {
            this.Exp += exp;
            while (this.Exp >= this.SkillConfig.Exp)
            {
                var upExp = this.SkillConfig.Exp;
                this.Level++;
                this.Exp -= upExp;
            }
        }
        //----------------
    }

    public enum SkillType
    {
        Attack = 1, //ֱ�ӹ�������
        Valet = 2, //�ٻ�����
        Map =3, //�������ܣ������ǽ��
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

    public enum SkillPosition
    {
        Last = 8,
        Default = 9
    }
}
