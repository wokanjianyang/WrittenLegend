using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using UnityEngine;
using Random = System.Random;

namespace Game
{
    public static class AppHelper
    {
        public static int DefendLevel = 0;

        public static string getKey()
        {
#if UNITY_EDITOR
            return "fb2d1feffd645dae1c574954fd702a80";
#endif
            //string pn = Application.identifier;
            //pn = EncryptionHelper.AesEncrypt(pn) + EncryptionHelper.Md5(pn + "8932kMD5#>>");
            //if (pn != "CZiSFbEnJLzHUa2n4QiF3a5EgGe+458f4EBvGvm+xZQ=ebe5d8b49fc4c8e07ebb7ddf8cb95fa5")
            //{
            //	return false;
            //}
            //UserData.pn = EncryptionHelper.Md5(pn + "z1!");

            // 获取Android的PackageManager    
            AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject Activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");

            // 获取当前Android应用的包名
            string packageName = Activity.Call<string>("getPackageName");

            // 调用PackageManager的getPackageInfo方法来获取签名信息数组    
            int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
            AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
            AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");

            // 获取当前的签名的哈希值，判断其与我们签名的哈希值是否一致
            if (Signatures != null && Signatures.Length > 0)
            {
                byte[] bytes = Signatures[0].Call<byte[]>("toByteArray");

                string hashCode = EncryptionHelper.GetMD5(bytes).ToUpper();

                hashCode = EncryptionHelper.Md5(hashCode + "12sd#$kd0z54");

                //UserData.sk = EncryptionHelper.Md5(hashCode + "#2A");

                return hashCode;
            }

            return null;
        }


        //获取设备标识符
        public static string GetDeviceIdentifier()
        {
            string s = SystemInfo.deviceUniqueIdentifier;
            s = EncryptionHelper.Md5(s);
            s = s.Substring(0, 10).ToUpper();

            return s;
        }

        public static int InitSeed()
        {
            return RandomHelper.RandomNumber(1, 123456789); ;
        }

        public static int RefreshSeed(int seed)
        {
            if (seed <= 0)
            {
                return RandomHelper.RandomNumber(1, int.MaxValue - 1);
            }

            return seed + 1;
        }
        public static int RefreshSeed1(int seed)
        {
            if (seed <= 0)
            {
                return RandomHelper.RandomNumber(1, int.MaxValue - 1);
            }

            return seed - 1;
        }

        public static int RefreshDaySeed(int seed)
        {
            if (seed <= 0)
            {
                return RandomHelper.RandomNumber(1, int.MaxValue - 1);
            }

            int todaySeed = Math.Abs(seed + TimeHelper.TodaySeed());
            return RandomHelper.RandomNumber(todaySeed, 1, int.MaxValue - 1);
        }

        public static int RefreshWeekSeed(int seed)
        {
            if (seed <= 0)
            {
                return RandomHelper.RandomNumber(1, int.MaxValue - 1);
            }

            int todaySeed = Math.Abs(seed + TimeHelper.WeekSeed());
            return RandomHelper.RandomNumber(todaySeed, 1, int.MaxValue - 1);
        }
    }
}