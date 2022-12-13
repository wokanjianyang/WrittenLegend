using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Com_Item : MonoBehaviour
    {
        private Item item;

        public int boxId { get; private set; }

        public Com_Box comBox;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItem(Item item)
        {
            this.item = item;

            this.comBox.tmp_Title.text = item.Name;

            this.comBox.Item = item;
        }

        public void SetBoxId(int id)
        {
            this.boxId = id;
        }    
    }
}