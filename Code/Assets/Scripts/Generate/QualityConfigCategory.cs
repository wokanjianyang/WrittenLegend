using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class QualityConfigCategory
    {

    }

    public class QualityConfigHelper
    {
        public static string GetColor(Item item)
        {
            var titleColor = GetQualityColor(item.GetQuality());
            return titleColor;
        }

        public static string GetEquipTagColor(bool isKeep)
        {
            return isKeep ? "FF0000" : "FFFFFF";
        }

        public static string GetQualityColor(int quality)
        {
            var titleColor = "FFFFFF";

            switch (quality)
            {
                case 1:
                    titleColor = "CCCCCC";
                    break;
                case 2:
                    titleColor = "CBFFC2";
                    break;
                case 3:
                    titleColor = "76B0FF";
                    break;
                case 4:
                    titleColor = "EE66EE";
                    break;
                case 5:
                    titleColor = "FF6600";
                    break;
                case 6:
                    titleColor = "E60000";
                    break;
            }

            return titleColor;
        }

        public static string GetMsgColor(MsgType type)
        {
            string color = "FFFFFF";

            switch (type)
            {
                case MsgType.Damage:
                    color = "FF0000";
                    break;
                case MsgType.Restore:
                    color = "00A86B";
                    break;
                case MsgType.Crit:
                    color = "FFD700";
                    break;
                case MsgType.Effect:
                    color = "E3EA6F";
                    break;
                case MsgType.Other:
                    break;
            }

            return color;
        }

        public static string GetTaskColor(bool over)
        {
            if (over)
            {
                return "00FF00";
            }
            else
            {
                return "FFFFFF";
            }

        }

        public static string GetEquipGroupColor(bool over)
        {
            if (over)
            {
                return "FEFE00";
            }
            else
            {
                return "CCCCCC";
            }

        }
    }
}