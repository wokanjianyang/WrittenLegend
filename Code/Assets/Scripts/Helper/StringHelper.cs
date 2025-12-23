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

            List<int> percents = ConfigHelper.BaseAttrIdList.ToList();
            //List<int> rates = ConfigHelper.RateAttrIdList.ToList();

            if (attrId == 2011 && val > 99.99999999 && val < 100)
            {
                return BuildMulResist(val);
            }

            if (!percents.Contains(attrId))
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

        private const int Start = 1;

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
                int unitIndex = Math.Min(index,ConfigHelper.UnitList.Length);
                index -= unitIndex;
                unit = ConfigHelper.UnitList[unitIndex - 1] + unit;
            }

            //加上点
            if (src.Length <= 3)
            {
                string scale = val.Substring(src.Length, 3 - src.Length).TrimEnd('0');
                if (scale.Length > 0) //小数位全是0,不显示
                {
                    src += "." + scale;
                }
            }

            return src + unit;
        }

        public static double StringToNumber(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return 0;
            }

            int e = 0;
            string res = "";

            for (int i = 0; i < text.Length; i++)
            {
                string t = text[i] + "";
                int index = Array.IndexOf(ConfigHelper.UnitList, t);

                if (index >= 0)
                {
                    e += (index + 1) * 4;
                }
                else
                {
                    res += t;
                }
            }

            if (e > 0)
            {
                res += "E" + e;
            }

            return Convert.ToDouble(res);
        }


        private static string[] LayerChinaList = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十",
            "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八","十九", "二十" };

        public static readonly Dictionary<char, char> NumberDict = new Dictionary<char, char>
        {
            { '1', '一' },{ '2', '二' },{ '3', '三' },{ '4', '四' },{ '5', '五' },
            { '6', '六' },{ '7', '七' },{ '8', '八' },{ '9', '九' },{ '0', '零' },
        };


        public static string GetChinaNumber(long number)
        {
            if (number <= 10)
            {
                return LayerChinaList[number];
            }
            else
            {
                string name = number + "";
                string temp = "";

                for (int i = 0; i < name.Length; i++)
                {
                    temp += NumberDict[name[i]];
                }
                return temp;
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