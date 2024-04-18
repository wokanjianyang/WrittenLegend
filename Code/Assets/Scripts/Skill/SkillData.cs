using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillData
    {
        public int SkillId { get; set; }
        public MagicData MagicExp { get; set; } = new MagicData();
        public MagicData MagicLevel { get; set; } = new MagicData();

        //技能状态
        public SkillStatus Status { get; set; }

        //装配位置
        public int Position { get; set; }

        public bool Recovery { get; set; } = false;

        [JsonIgnore]
        public SkillConfig SkillConfig { get; set; }

        public long GetLevelUpExp()
        {
            long rate = 50;

            long tempLevel = MagicLevel.Data;

            if (tempLevel < 100)
            {
                rate = Math.Min(10, tempLevel + 5);
            }
            else if (tempLevel >= 100 && tempLevel < 150)
            {
                rate = 20;
            }
            else if (tempLevel >= 150 && tempLevel < 200)
            {
                rate = 30;
            }
            else if (tempLevel >= 200 && tempLevel < 250)
            {
                rate = 40;
            }
            else
            {
                rate = 50;
            }

            //if (tempLevel >= SkillConfig.MaxLevel)
            //{
            //    rate = 9999999;
            //}

            return rate * SkillConfig.Exp;
        }

        public SkillData(int skillId, int position)
        {
            this.Position = position;
            SkillConfig = SkillConfigCategory.Instance.Get(skillId);
            this.SkillId = SkillConfig.SkillId;
        }

        public void AddExp(long exp)
        {
            User user = GameProcessor.Inst.User;

            this.MagicExp.Data += exp;
            while (this.MagicExp.Data >= GetLevelUpExp() && this.MagicLevel.Data < user.GetSkillLimit(SkillConfig))
            {
                var upExp = GetLevelUpExp();
                this.MagicLevel.Data++;
                this.MagicExp.Data -= upExp;
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
        Yeman = 7,//野蛮
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
