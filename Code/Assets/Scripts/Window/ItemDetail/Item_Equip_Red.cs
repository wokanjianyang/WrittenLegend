using Game.Data;
using Sirenix.OdinInspector;
using System;
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

        public void SetContent(EquipRedItem redItem, int quality)
        {
            string color = redItem.Count >= redItem.Config.Count ? QualityConfigHelper.GetQualityColor(quality) : "CCCCCC";

            int showLevel = Math.Max(1, redItem.Level);

            string qn = "��װ";
            if (quality == 7)
            {
                qn = "��װ";
            }

            string name = ConfigHelper.LayerChinaList[showLevel] + "��" + qn + string.Format("({0}/{1})", redItem.Count, redItem.Config.Count);

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", color, name);

            int attr = redItem.Config.AttrValue + (showLevel - 1) * redItem.Config.AttrRise;

            this.Txt_Des.text = string.Format("<color=#{0}>{1}</color>", color, StringHelper.FormatAttrText(redItem.Config.AttrId, attr, "+"));
        }
    }
}
