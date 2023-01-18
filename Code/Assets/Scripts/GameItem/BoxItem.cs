using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class BoxItem
    {
        public BoxItem() {

        }

        public Item Item { get; set; }

        public int Number { get; set; } = 0;

        public int BoxId { get; set; }

        public void AddStack()
        {
            this.Number++;
        }

        public void RemoveStack() {
            this.Number--;
        }

        public bool IsFull() {
            if (Number < Item.MaxNum) {
                return false;
            }

            return true;
        }
    }
}