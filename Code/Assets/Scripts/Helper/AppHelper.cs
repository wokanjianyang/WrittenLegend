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

        public static int BabelRecord = 0;

        public static int SaveFailCount = 0;

        public static bool PetEgging = false;

        public static int TempRecord = 0;
        public static int TempRecord1 = 0;

        public static int CopyCount = 0;
        public static int HundunCount = 0;

        public const int EquipHundun_MaxDropId = 1000001; //混沌装备，保底id
        public const int EquipHundun_MaxCount = 8000; //混沌装备，保底数量
        public const int EquipHundun_MinRate = 100; //保底概率

        public static int TestExclusive2 = 0;
        public static int TestExclusive3 = 0;

        public static bool Shengxiao_Auto = false;
        public static int Shengxiao_Id = 1;

        public static bool Spirit_Auto = false;
        public static int Spirit_Id = 1;

        public static bool LegacyAuto = true;

        public static int GetLossQuality()
        {
            //如果次数少于500次，则品质-1
            return CopyCount > 600 ? 0 : 1;
        }

        public static string getKey()
        {
            return "fb2d1feffd645dae1c574954fd702a80";

            //#if UNITY_EDITOR
            //            return "fb2d1feffd645dae1c574954fd702a80";
            //#endif
            //            //string pn = Application.identifier;
            //            //pn = EncryptionHelper.AesEncrypt(pn) + EncryptionHelper.Md5(pn + "8932kMD5#>>");
            //            //if (pn != "CZiSFbEnJLzHUa2n4QiF3a5EgGe+458f4EBvGvm+xZQ=ebe5d8b49fc4c8e07ebb7ddf8cb95fa5")
            //            //{
            //            //	return false;
            //            //}
            //            //UserData.pn = EncryptionHelper.Md5(pn + "z1!");

            //            // 获取Android的PackageManager    
            //            AndroidJavaClass Player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //            AndroidJavaObject Activity = Player.GetStatic<AndroidJavaObject>("currentActivity");
            //            AndroidJavaObject PackageManager = Activity.Call<AndroidJavaObject>("getPackageManager");

            //            // 获取当前Android应用的包名
            //            string packageName = Activity.Call<string>("getPackageName");

            //            // 调用PackageManager的getPackageInfo方法来获取签名信息数组    
            //            int GET_SIGNATURES = PackageManager.GetStatic<int>("GET_SIGNATURES");
            //            AndroidJavaObject PackageInfo = PackageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, GET_SIGNATURES);
            //            AndroidJavaObject[] Signatures = PackageInfo.Get<AndroidJavaObject[]>("signatures");

            //            // 获取当前的签名的哈希值，判断其与我们签名的哈希值是否一致
            //            if (Signatures != null && Signatures.Length > 0)
            //            {
            //                byte[] bytes = Signatures[0].Call<byte[]>("toByteArray");

            //                string hashCode = EncryptionHelper.GetMD5(bytes).ToUpper();

            //                hashCode = EncryptionHelper.Md5(hashCode + "12sd#$kd0z54");

            //                //UserData.sk = EncryptionHelper.Md5(hashCode + "#2A");

            //                return hashCode;
            //            }

            //            return null;
        }

        public static string getSourcePath()
        {

            try
            {
                AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = player.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject packageManager = activity.Call<AndroidJavaObject>("getPackageManager");
                string packageName = activity.Call<string>("getPackageName");
                AndroidJavaObject packageInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
                string apkPath = packageInfo.Get<AndroidJavaObject>("applicationInfo").Get<string>("sourceDir");

                return apkPath;
            }
            catch (Exception e)
            {
                return "获取SourcePath失败";
            }
        }

        public static bool HasOverlayPermission()
        {
#if UNITY_EDITOR
            return false;
#else

            try
            {
                AndroidJavaClass contextClass = new AndroidJavaClass("android.content.Context");
                AndroidJavaObject unityActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject appContext = unityActivity.Call<AndroidJavaObject>("getApplicationContext");
                AndroidJavaClass settingsClass = new AndroidJavaClass("android.provider.Settings");
                bool canDraw = settingsClass.CallStatic<bool>("canDrawOverlays", appContext);
                return canDraw;
            }
            catch (Exception e)
            {
                return false;
            }
#endif
        }

        public static string GetBaseMd5()
        {
#if UNITY_EDITOR
            return "editor";
#else

            try
            {
                string apkPath = Application.dataPath + "/../base.apk"; ;
                if (File.Exists(apkPath))
                {
                    long fileSizeBytes = new FileInfo(apkPath).Length;
                    return EncryptionHelper.Md5(fileSizeBytes + ""); ;
                }
                else
                {
                    return "notfile";
                }
            }
            catch (Exception e)
            {
                return "notfile";
            }
#endif
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

        public static string GetCode()
        {
            string dllPath = Path.Combine(Application.dataPath, "/com.lcgame.wujinanyu/files/il2cpp/Managed/libil2cpp.so");

            string hash = "";

            if (!File.Exists(dllPath))
            {
                return "路径不存在";
            }

            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream stream = File.OpenRead(dllPath))
                {
                    byte[] hashBytes = sha256.ComputeHash(stream);
                    hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }

            Debug.Log($"Assembly-CSharp.dll哈希值：{hash}");

            return hash;
        }
    }
}