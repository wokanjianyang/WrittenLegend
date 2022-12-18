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
        /// Ĭ��
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
