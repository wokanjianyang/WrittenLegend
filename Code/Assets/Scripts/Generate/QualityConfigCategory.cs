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
                    titleColor = "CBFFC2";
                    break;
                case 2:
                    titleColor = "CCCCCC";
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

    }
}