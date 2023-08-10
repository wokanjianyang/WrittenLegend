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

        //技能状态
        public SkillStatus Status { get; set; }

        //装配位置
        public int Position { get; set; }

        [JsonIgnore]
        public SkillConfig SkillConfig { get; set; }

        public int GetLevelUpExp()
        {
            int rate = 9999999;

            if (Level < 100)
            {
                rate = Mathf.Min(10, Level + 5);
            }
            else if (Level >= 100 && Level < 150)
            {
                rate = 20;
            }
            else if (Level >= 150 && Level < 200)
            {
                rate = 30;
            }
            else if (Level >= 200 && Level < 250)
            {
                rate = 40;
            }
            else if (Level >= 250 && Level < 300)
            {
                rate = 50;
            }
            else if (Level >= 350 && Level < 400)
            {
                rate = 75;
            }

            return rate * SkillConfig.Exp;
        }

        public SkillData(int skillId, int position)
        {
            this.SkillId = skillId;
            this.Position = position;
            SkillConfig = SkillConfigCategory.Instance.Get(skillId);
        }

        public void AddExp(long exp)
        {
            this.Exp += exp;
            while (this.Exp >= GetLevelUpExp())
            {
                var upExp = GetLevelUpExp();
                this.Level++;
                this.Exp -= upExp;
            }
        }
        //----------------
    }

    public enum SkillType
    {
        Attack = 1,  //直接攻击技能
        Valet = 2, //召唤技能
        Map = 3,  //场景技能（比如火墙）
        Restore = 4,//恢复技能
        Shield = 5,//状态技能
        Expert = 6,//职业专精技能
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

    public enum SkillPosition
    {
        Default = 999999
    }
}
