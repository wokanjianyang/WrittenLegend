using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Buff : MonoBehaviour
    {
        //public Image Img_Active;
        public Text Txt_Name;
        public Text Txt_Des;

        public Toggle toggle;

        public DefendBuffConfig Config { get; set; }


        // Update is called once per frame
        void Update()
        {

        }

        public void SetContent(DefendBuffConfig config)
        {
            this.Config = config;

            this.Txt_Name.text = config.Name;
            this.Txt_Des.text = string.Format(config.Memo, config.AttrValue);
        }
    }
}
