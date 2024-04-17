using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Item
    {
        public int Count { get; set; }

        public bool IsLock { get; set; }
        public bool IsNew { get; set; } = true;
        public bool IsKeep { get; set; } = false;

        public int Seed { get; set; } = -1;

        protected Item()
        {

        }
        public Item(int configId)
        {
            this.ConfigId = configId;
            ItemConfig = ItemConfigCategory.Instance.Get(this.ConfigId);

            this.Name = ItemConfig.Name;
            this.Des = ItemConfig.Des;
            this.Type = (ItemType)ItemConfig.Type;
            this.Level = ItemConfig.LevelRequired;
            this.Gold = ItemConfig.Price;
            this.MaxNum = ItemConfig.MaxNum;
            this.Count = 1;
        }

        public int ConfigId
        {
            get;
            set;
        }

        public void RefreshSeed()
        {
            this.Seed = AppHelper.RefreshSeed(this.Seed);
        }

        [JsonIgnore]
        public ItemConfig ItemConfig { get; set; }

        virtual public int GetQuality()
        {
            return ItemConfig.Quality;
        }

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
        public long Gold { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 堆叠数量
        /// </summary>
        public int MaxNum { get; set; }

        //[JsonIgnore]
        //public int BoxId { get; set; } = -1;
    }

    public enum ItemType
    {
        Normal = 0,
        Gold = 1,
        Equip = 2,
        SkillBox = 3,
        GiftPack = 4,
        Material = 5,
        Buff = 6,
        GoldPack = 7,
        ExpPack = 8,
        Ticket = 9,
        Exclusive = 10,
        Card = 11,
        GiftPackExclusive = 12,
        Fashion = 13,
        Halidom = 14,

        Artifact = 18,

        Metal = 98,
        Ad = 99,
    }
}
