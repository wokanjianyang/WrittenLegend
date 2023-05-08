using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GiftPack : Item
    {
        [JsonIgnore]
        public GiftPackConfig Config { get; set; }
        public GiftPack(int configId)
        {
            this.ConfigId = configId;
            this.Type = ItemType.GiftPack;

            Config = GiftPackConfigCategory.Instance.Get(configId);

            Name = Config.Name;
            Des = Config.Name;
            Level = Config.LevelRequired;
            Gold = 0;
        }

        public override int GetQuality()
        {
            return 4;
        }
    }
}
