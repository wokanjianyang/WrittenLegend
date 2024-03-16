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
        public static string FormatAttrText(int attrId, long val, string cr)
        {
            return FormatAttrValueName(attrId) + cr + FormatAttrValueText(attrId, val);
        }

        public static string FormatAttrValueText(int attrId, double val)
        {
            string nt = "";
            string unit = "";

            List<int> percents = ConfigHelper.PercentAttrIdList.ToList();
            //List<int> rates = ConfigHelper.RateAttrIdList.ToList();

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
                nt = val.ToString("0.####");
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

        private static string FormatNumber(string val, string unit)
        {
            string src;

            if (val.Length > 17)
            {
                unit = "京" + unit;
                src = val.Substring(0, val.Length - 16);
            }
            else if (val.Length > 13)
            {
                unit = "兆" + unit;
                src = val.Substring(0, val.Length - 12);
            }
            else if (val.Length > 9)
            {
                unit = "亿" + unit;
                src = val.Substring(0, val.Length - 8);
            }
            else if (val.Length > 5)
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
                string scale = val.Substring(src.Length, 4 - src.Length).TrimEnd('0');
                if (scale.Length > 0) //小数位全是0,不显示
                {
                    src += "." + scale;
                }
                return src + unit;
            }
            else
            {
                return FormatNumber(src, unit);
            }
        }
    }
}