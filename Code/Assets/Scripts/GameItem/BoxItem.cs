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
        public BoxItem()
        {

        }

        public Item Item { get; set; }

        public MagicData MagicNubmer { get; } = new MagicData();

        public int BoxId { get; set; }

        public void AddStack(long quantity)
        {
            this.MagicNubmer.Data += quantity;
        }

        public void RemoveStack(long quantity)
        {
            this.MagicNubmer.Data -= quantity;
        }

        public bool IsFull()
        {
            if (MagicNubmer.Data < Item.MaxNum)
            {
                return false;
            }

            return true;
        }

        public int GetBagType()
        {
            if (this.Item.Type == ItemType.Equip)
            {
                int type = (this.Item as Equip).EquipConfig.Role;

                return type <= 0 ? 3 : type - 1; //四格等全职业装备放战士包裹
            }
            if (this.Item.Type == ItemType.Exclusive)
            {
                return 3;
            }

            return 4;
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
