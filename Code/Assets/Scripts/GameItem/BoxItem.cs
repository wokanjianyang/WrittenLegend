using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Game.Data;
using System;

namespace Game
{
    public class BoxItem
    {
        public BoxItem() {

        }

        public Item Item { get; set; }

        public int Number { get; set; } = 0;

        public MagicData MagicNubmer { get; } = new MagicData();

        public int BoxId { get; set; }

        public void AddStack(long quantity)
        {
            this.MagicNubmer.Data += quantity;
        }

        public void RemoveStack(long quantity) {
            this.MagicNubmer.Data -= quantity;
        }

        public bool IsFull() {
            if (MagicNubmer.Data < Item.MaxNum) {
                return false;
            }

            return true;
        }

        public int GetBagType()
        {
            if (this.Item.Type == ItemType.Equip)
            {
                int type = (this.Item as Equip).EquipConfig.Role - 1;

                return Math.Max(type, 0); //四格等全职业装备放战士包裹
            }

            return 3;
        }

        public int GetBagSort()
        {
            if (this.Item.Type == ItemType.Equip)
            {
                var config = (this.Item as Equip).EquipConfig;
                return config.Part * 10000 + config.LevelRequired + config.Quality;
            }

            return this.Item.ConfigId;
        }
    }
}
