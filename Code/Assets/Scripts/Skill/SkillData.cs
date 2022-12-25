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

        //技能状态
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
        /// 施法类型 1 主动 2被动
        /// </summary>
        public int CastType { get; set; }

        /// <summary>
        /// 职业 1 战士 2 法师 3 道士
        /// </summary>
        public int Role { get; set; }

        public int Level { get; set; }

        /// <summary>
        /// 攻击距离
        /// </summary>
        public int Dis { get; set; }

        /// <summary>
        /// 计算中心 self enemy
        /// </summary>
        public SkillCenter Center { get; set; }

        /// <summary>
        /// 攻击区域类型
        /// </summary>
        public AttackGeometryType Area { get; set; }

        /// <summary>
        /// 最大攻击敌人数量
        /// </summary>
        public int EnemyMax { get; set; }

        /// <summary>
        /// 伤害比例
        /// </summary>
        public int Percent { get; set; }

        /// <summary>
        /// 固定伤害
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
        /// 未学习
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 已学习
        /// </summary>
        Learn = 1,
        /// <summary>
        /// 已装配
        /// </summary>
        Equip = 2,
    }
}
