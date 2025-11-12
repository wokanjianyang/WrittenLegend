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

            string qn = "红装";
            if (quality == 7)
            {
                qn = "金装";
            }
            else if (quality == 8)
            {
                qn = "暗金";
            }
            else if (quality == 9)
            {
                qn = "混沌";
            }

            string name = ConfigHelper.LayerChinaList[showLevel] + "阶" + qn + string.Format("({0}/{1})", redItem.Count, redItem.Config.Count);

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", color, name);

            int attr = (int)(redItem.Config.AttrValue + (showLevel - 1) * redItem.Config.AttrRise);

            this.Txt_Des.text = string.Format("<color=#{0}>{1}</color>", color, StringHelper.FormatAttrText(redItem.Config.AttrId, attr, "+"));
        }


        public void SetShengxiaoGroup(ShengxiaoGroupItem item)
        {
            string color = item.Count >= item.Config.Count ? QualityConfigHelper.GetQualityColor(item.Config.Quality) : "CCCCCC";

            string qn = "红色";
            if (item.Config.Quality == 7)
            {
                qn = "金色";
            }
            else if (item.Config.Quality == 8)
            {
                qn = "暗金";
            }
            else if (item.Config.Quality == 9)
            {
                qn = "粉色";
            }

            string name = qn + "生肖" + string.Format("({0}/{1})", item.Count, item.Config.Count);

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", color, name);

            int attr = (int)(item.Config.AttrValue);

            this.Txt_Des.text = string.Format("<color=#{0}>{1}</color>", color, StringHelper.FormatAttrText(item.Config.AttrId, attr, "+"));
        }
    }
}
