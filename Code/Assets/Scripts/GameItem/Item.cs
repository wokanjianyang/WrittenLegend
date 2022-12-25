using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Item
    {
        public Item() { 

        }
        public Item(int configId) {
            this.ConfigId = configId;
            ItemConfig = ItemConfigCategory.Instance.Get(this.ConfigId);
            this.Quality = ItemConfig.Quality;

            this.Name = ItemConfig.Name;
            this.Des = ItemConfig.Des;
            this.Type = (ItemType)ItemConfig.Type;
            this.Level = ItemConfig.LevelRequired;
            this.Gold = ItemConfig.Price;
            this.MaxNum = ItemConfig.MaxNum;
        }

        public int ConfigId
        {
            get;
            set;
        }

        [JsonIgnore]
        /// <summary>
        /// 道具品质，用做显示或者其他
        /// </summary>
        public int Quality { get; set; }

        [JsonIgnore]
        public ItemConfig ItemConfig { get; set; }

        [JsonIgnore]
        public string Name { get; set; }

        [JsonIgnore]
        public string Des { get; set; }

        [JsonIgnore]
        /// <summary>
        ///  道具类型
        /// </summary>
        public ItemType Type { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 装备所需等级
        /// </summary>
        public int Level { get; set; }

        [JsonIgnore]
        public int Gold { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 堆叠数量
        /// </summary>
        public int MaxNum { get; set; }

        [JsonIgnore]
        public int BoxId { get; set; } = -1;
    }

    public enum ItemType { 
        Normal = 0,
        Gold = 1,
        Equip = 2,
        SkillBox =3
    }
}
