using System;
using UnityEngine;

namespace Game
{
    public static class ColorHelper
    {
        public static Color HexToColor(string hex)
        {
            // 移除前缀#符号（如果有）
            if (hex.StartsWith("#"))
                hex = hex.Substring(1);

            // 确保十六进制字符串的长度是6或8（对于带有透明度的情况）
            if (hex.Length != 6 && hex.Length != 8)
                throw new System.ArgumentException("Hex string must be 6 or 8 characters long", "hex");

            // 将十六进制字符串转换为byte数组
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = hex.Length == 8 ? (byte)byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;

            // 使用转换后的byte值创建Color对象
            return new Color32(r, g, b, a);
        }

        public static Color GetColorByQuality(int quality) {
            string color = QualityConfigHelper.GetQualityColor(quality);
            return HexToColor(color);
        }
    }
}