using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Equip_Red : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Des;

        public void SetContent(int count, int redLevel, EquipRedConfig config)
        {

            string color = count >= config.Count ? "FF0000" : "CCCCCC";

            string name = (redLevel) + "½×ºì×°" + string.Format("({0}/{1})", count, config.Count);

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", color, name);

            this.Txt_Des.text = string.Format("<color=#{0}>{1}</color>", color, StringHelper.FormatAttrText(config.AttrId, config.AttrValue + (redLevel - 1) * config.AttrRise, "+"));
        }
    }
}
