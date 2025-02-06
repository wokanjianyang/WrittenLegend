using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Game
{
    public static class StringHelper
    {
        public static IEnumerable<byte> ToBytes(this string str)
        {
            byte[] byteArray = Encoding.Default.GetBytes(str);
            return byteArray;
        }

        public static byte[] ToByteArray(this string str)
        {
            byte[] byteArray = Encoding.Default.GetBytes(str);
            return byteArray;
        }

        public static byte[] ToUtf8(this string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            return byteArray;
        }

        public static byte[] HexToBytes(this string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            var hexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < hexAsBytes.Length; index++)
            {
                string byteValue = "";
                byteValue += hexString[index * 2];
                byteValue += hexString[index * 2 + 1];
                hexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return hexAsBytes;
        }

        //public static string FormatPhantomText(int rewardId, int number)
        //{
        //    return FormatAttrValueName(rewardId) + "+" + FormatAttrValueText(rewardId, number);
        //}

        public static string FormatAttrValueName(int attrId)
        {
            return PlayerHelper.PlayerAttributeMap[((AttributeEnum)attrId).ToString()];
        }

        public static string FormatAttrText(int attrId, long val)
        {
            return FormatAttrText(attrId, val, "");
        }
        public static string FormatAttrText(int attrId, double val)
        {
            return FormatAttrValueName(attrId) + "" + FormatAttrValueText(attrId, val);
        }

        public static string FormatAttrText(int attrId, long val, string cr)
        {
            return FormatAttrValueName(attrId) + cr + FormatAttrValueText(attrId, val);
        }

        public static string BuildMulResist(double val)
        {
            string text = val + "";

            int count = 2;
            for (int i = 0; i <= text.Length - 4; i++)
            {
                string c = text.Substring(i + 3, 1);
                if (c == "9")
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count + "个9";
        }

        public static string FormatAttrValueText(int attrId, double val)
        {
            string nt = "";
            string unit = "";

            List<int> percents = ConfigHelper.PercentAttrIdList.ToList();
            //List<int> rates = ConfigHelper.RateAttrIdList.ToList();

            if (attrId == 2011 && val > 99.99999999 && val < 100)
            {
                return BuildMulResist(val);
            }

            if (percents.Contains(attrId))
            {
                unit = "%";
            }

            if (val >= 10000000)
            {
                nt = StringHelper.FormatNumber(val);
            }
            else
            {
                nt = val.ToString("0.########");
            }

            return nt + unit;
        }

        public static string Fmt(this string text, params object[] args)
        {
            return string.Format(text, args);
        }

        public static string ListToString<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T t in list)
            {
                sb.Append(t);
                sb.Append(",");
            }
            return sb.ToString();
        }

        public static string ArrayToString<T>(this T[] args)
        {
            if (args == null)
            {
                return "";
            }

            string argStr = " [";
            for (int arrIndex = 0; arrIndex < args.Length; arrIndex++)
            {
                argStr += args[arrIndex];
                if (arrIndex != args.Length - 1)
                {
                    argStr += ", ";
                }
            }

            argStr += "]";
            return argStr;
        }

        public static string ArrayToString<T>(this T[] args, int index, int count)
        {
            if (args == null)
            {
                return "";
            }

            string argStr = " [";
            for (int arrIndex = index; arrIndex < count + index; arrIndex++)
            {
                argStr += args[arrIndex];
                if (arrIndex != args.Length - 1)
                {
                    argStr += ", ";
                }
            }

            argStr += "]";
            return argStr;
        }

        public static int[] ConvertSkillParams(string param)
        {
            string[] list = param.Split(",", StringSplitOptions.RemoveEmptyEntries);
            int[] result = new int[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                result[i] = Convert.ToInt32(list[i]);
            }

            return result;
        }

        public static string FormatNumber(long val)
        {
            return FormatNumber(val.ToString(), "");
        }

        public static string FormatNumber(double val)
        {
            return FormatNumber(val.ToString("0"), "");
        }

        private static string[] UnitList = { "万", "亿", "兆", "京", "垓", "秭", "穰", "沟", "涧", "正", "载", "极", "恒", "河", "沙", "阿", "僧", "祇"
                , "那", "由", "他", "不", "可", "思" , "议","无","量","大","数‌" };

        private const int Start = 0;

        private static string FormatNumber(string val, string unit)
        {
            if (val.Length <= 4)
            {
                return val + unit;
            }

            int index = (val.Length - Start) / 4;
            string src = val.Substring(0, val.Length - index * 4);

            while (index > 0)
            {
                int unitIndex = Math.Min(index, UnitList.Length);
                index -= unitIndex;
                unit = UnitList[unitIndex - 1] + unit;
            }

            //加上点
            string scale = val.Substring(src.Length, 3 - src.Length).TrimEnd('0');
            if (scale.Length > 0) //小数位全是0,不显示
            {
                src += "." + scale;
            }
            return src + unit;
        }

        private static string FormatNumberOld(string val, string unit)
        {
            string src;

            if (val.Length > 116 + Start)
            {
                unit = "数" + unit;
                src = val.Substring(0, val.Length - 116);
            }
            else if (val.Length > 112 + Start)
            {
                unit = "大" + unit;
                src = val.Substring(0, val.Length - 112);
            }
            else if (val.Length > 108 + Start)
            {
                unit = "量" + unit;
                src = val.Substring(0, val.Length - 108);
            }
            else if (val.Length > 104 + Start)
            {
                unit = "无" + unit;
                src = val.Substring(0, val.Length - 104);
            }
            else if (val.Length > 100 + Start)
            {
                unit = "议" + unit;
                src = val.Substring(0, val.Length - 100);
            }
            else if (val.Length > 96 + Start)
            {
                unit = "思" + unit;
                src = val.Substring(0, val.Length - 96);
            }
            else if (val.Length > 92 + Start)
            {
                unit = "可" + unit;
                src = val.Substring(0, val.Length - 92);
            }
            else if (val.Length > 88 + Start)
            {
                unit = "不" + unit;
                src = val.Substring(0, val.Length - 88);
            }
            else if (val.Length > 84 + Start)
            {
                unit = "他‌" + unit;
                src = val.Substring(0, val.Length - 84);
            }
            else if (val.Length > 80 + Start)
            {
                unit = "由" + unit;
                src = val.Substring(0, val.Length - 80);
            }
            else if (val.Length > 76 + Start)
            {
                unit = "那" + unit;
                src = val.Substring(0, val.Length - 76);
            }
            else if (val.Length > 72 + Start)
            {
                unit = "祇" + unit;
                src = val.Substring(0, val.Length - 72);
            }
            else if (val.Length > 68 + Start)
            {
                unit = "僧" + unit;
                src = val.Substring(0, val.Length - 68);
            }
            else if (val.Length > 64 + Start)
            {
                unit = "阿" + unit;
                src = val.Substring(0, val.Length - 64);
            }
            else if (val.Length > 60 + Start)
            {
                unit = "沙" + unit;
                src = val.Substring(0, val.Length - 60);
            }
            else if (val.Length > 56 + Start)
            {
                unit = "河" + unit;
                src = val.Substring(0, val.Length - 56);
            }
            else if (val.Length > 52 + Start)
            {
                unit = "恒" + unit;
                src = val.Substring(0, val.Length - 52);
            }
            else if (val.Length > 48 + Start)
            {
                unit = "极" + unit;
                src = val.Substring(0, val.Length - 48);
            }
            else if (val.Length > 44)
            {
                unit = "载" + unit;
                src = val.Substring(0, val.Length - 44);
            }
            else if (val.Length > 40)
            {
                unit = "正" + unit;
                src = val.Substring(0, val.Length - 40);
            }
            else if (val.Length > 36)
            {
                unit = "涧" + unit;
                src = val.Substring(0, val.Length - 36);
            }
            else if (val.Length > 32)
            {
                unit = "沟" + unit;
                src = val.Substring(0, val.Length - 32);
            }
            else if (val.Length > 28)
            {
                unit = "穰" + unit;
                src = val.Substring(0, val.Length - 28);
            }
            else if (val.Length > 24)
            {
                unit = "秭" + unit;
                src = val.Substring(0, val.Length - 24);
            }
            else if (val.Length > 20)
            {
                unit = "垓" + unit;
                src = val.Substring(0, val.Length - 20);
            }
            else if (val.Length > 16)
            {
                unit = "京" + unit;
                src = val.Substring(0, val.Length - 16);
            }
            else if (val.Length > 12)
            {
                unit = "兆" + unit;
                src = val.Substring(0, val.Length - 12);
            }
            else if (val.Length > 8)
            {
                unit = "亿" + unit;
                src = val.Substring(0, val.Length - 8);
            }
            else if (val.Length > 4)
            {
                unit = "万" + unit;
                src = val.Substring(0, val.Length - 4);
            }
            else
            {
                return val + unit;
            }

            if (src.Length < 4)
            {   //加上点
                string scale = val.Substring(src.Length, 3 - src.Length).TrimEnd('0');
                if (scale.Length > 0) //小数位全是0,不显示
                {
                    src += "." + scale;
                }
                return src + unit;
            }
            else
            {
                return FormatNumberOld(src, unit);
            }
        }

        //private static string FormatNumberOld(string val, string unit)
        //{
        //    string src;

        //    if (val.Length > 49)
        //    {
        //        unit = "极" + unit;
        //        src = val.Substring(0, val.Length - 48);
        //    }
        //    else if (val.Length > 45)
        //    {
        //        unit = "载" + unit;
        //        src = val.Substring(0, val.Length - 44);
        //    }
        //    else if (val.Length > 41)
        //    {
        //        unit = "正" + unit;
        //        src = val.Substring(0, val.Length - 40);
        //    }
        //    else if (val.Length > 37)
        //    {
        //        unit = "涧" + unit;
        //        src = val.Substring(0, val.Length - 36);
        //    }
        //    else if (val.Length > 33)
        //    {
        //        unit = "沟" + unit;
        //        src = val.Substring(0, val.Length - 32);
        //    }
        //    else if (val.Length > 29)
        //    {
        //        unit = "穰" + unit;
        //        src = val.Substring(0, val.Length - 28);
        //    }
        //    else if (val.Length > 25)
        //    {
        //        unit = "秭" + unit;
        //        src = val.Substring(0, val.Length - 24);
        //    }
        //    else if (val.Length > 21)
        //    {
        //        unit = "垓" + unit;
        //        src = val.Substring(0, val.Length - 20);
        //    }
        //    else if (val.Length > 17)
        //    {
        //        unit = "京" + unit;
        //        src = val.Substring(0, val.Length - 16);
        //    }
        //    else if (val.Length > 13)
        //    {
        //        unit = "兆" + unit;
        //        src = val.Substring(0, val.Length - 12);
        //    }
        //    else if (val.Length > 9)
        //    {
        //        unit = "亿" + unit;
        //        src = val.Substring(0, val.Length - 8);
        //    }
        //    else if (val.Length > 5)
        //    {
        //        unit = "万" + unit;
        //        src = val.Substring(0, val.Length - 4);
        //    }
        //    else
        //    {
        //        return val + unit;
        //    }

        //    if (src.Length < 4)
        //    {   //加上点
        //        string scale = val.Substring(src.Length, 4 - src.Length).TrimEnd('0');
        //        if (scale.Length > 0) //小数位全是0,不显示
        //        {
        //            src += "." + scale;
        //        }
        //        return src + unit;
        //    }
        //    else
        //    {
        //        return FormatNumber(src, unit);
        //    }
        //}
    }
}