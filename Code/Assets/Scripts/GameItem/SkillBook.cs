using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillBook : Item
    {
        public SkillBook() {
            this.Type = ItemType.SkillBox;
        }
        public int Position { get; set; }

        public long Exp { get; set; }

        [JsonIgnore]
        public ItemConfig Config { get; set; }

        [JsonIgnore]
        public SkillConfig BookConfig { get; set; }

        public SkillBookType SkillType { get; set; } = SkillBookType.Normal;

        public int EquipBoxId { get; set; }

        public void AddExp(long exp)
        {
            this.Exp += exp;
            while(this.Exp>=this.GetUpExp())
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
    }
    public enum SkillBookType
    {
        /// <summary>
        /// 默认
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
