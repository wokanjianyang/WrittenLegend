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
        public static string GetColor(int quanlity)
        {
            var titleColor = "FFFFFF";

            switch (quanlity)
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
                    titleColor = "D800FF";
                    break;
            }

            return titleColor;
        }

        public static string GetMsgColor(MsgType type) {
            string color = "FFFFFF";

            switch (type) {
                case MsgType.Damage:
                    color = "FF0000";
                    break;
                case MsgType.Restore:
                    color = "44FF44";
                    break;
                case MsgType.Crit:
                    color = "E3EA6F";
                    break;
                case MsgType.Effect:
                    color = "FFD700";
                    break;
                case MsgType.Other:
                    break;
            }

            return color;
        }
    }
}