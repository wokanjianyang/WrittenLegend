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
            this.Txt_Name.text = (redLevel) + "½×ºì×°" + string.Format("({0}/{1})", count, config.Count);

            this.Txt_Des.text = "10%¹¥»÷";
        }
    }
}
