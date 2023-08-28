using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Game.Data;

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
    }
}
