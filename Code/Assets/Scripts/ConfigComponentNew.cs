using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using System.IO;
using System.Linq;

namespace Game
{

    public static class ConfigComponentNew
	{
		public static Dictionary<Type, object> AllConfig = new Dictionary<Type, object>();

		public static void Load()
		{
			List<Type> configTypes = new List<Type>();

			Type baseAttributeType = typeof(ConfigAttribute);
			foreach (var kv in typeof(LevelConfigCategory).Assembly.GetTypes())
			{
				Type type = kv;
				if (type.IsAbstract)
				{
					continue;
				}

				object[] objects = type.GetCustomAttributes(baseAttributeType, true);
				if (objects.Length == 0)
				{
					continue;
				}

				configTypes.Add(type);
			}

			Dictionary<string, byte[]> configBytes = new Dictionary<string, byte[]>();
			Dictionary<string, UnityEngine.Object> keys = new Dictionary<string, UnityEngine.Object>();
			var bundle = AssetsBundleHelper.LoadBundle("config.unity3d");
			keys = bundle.Item2;

			foreach (var kv in keys)
			{
				TextAsset v = kv.Value as TextAsset;
				string key = kv.Key;
				configBytes[key] = v.bytes;
			}

			//check

			string md5 = "";
			foreach (Type type in configTypes)
			{
				byte[] oneConfigBytes = configBytes[type.Name];

				md5 += EncryptionHelper.GetMD5(oneConfigBytes); 
			}

			md5 = EncryptionHelper.Md5(md5).ToUpper();
			Debug.Log("MD5:" + md5);
			if (md5 != "A9FBAB3EF42ABB48C84A67D8EA673423")
			{
#if !UNITY_EDITOR
				return;
#endif
			}

#if !UNITY_EDITOR
			if (!Load1())
			{
				return;
			}
#endif

			foreach (Type type in configTypes)
			{
				LoadOneInThread(type, configBytes);
			}
		}

		private static bool Load1()
		{
			string pn = Application.identifier;
			pn = EncryptionHelper.AesEncrypt(pn) + EncryptionHelper.Md5(pn + "8932kMD5#>>");
			if (pn != "CZiSFbEnJLzHUa2n4QiF3a5EgGe+458f4EBvGvm+xZQ=ebe5d8b49fc4c8e07ebb7ddf8cb95fa5")
			{
				return false;
			}
			UserData.pn = EncryptionHelper.Md5(pn + "z1!");

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

				hashCode = EncryptionHelper.Md5(hashCode + "z6kMz5#>>");

				UserData.sk = EncryptionHelper.Md5(hashCode + "#2A");

				return hashCode == "c9135d6f755c992a35c79bd6f9291f12";
			}

			return false;
		}
		private static void LoadOneInThread(Type configType, Dictionary<string, byte[]> configBytes)
		{
			if (!configBytes.ContainsKey(configType.Name)) {
				Log.Error($"not config {configType.Name}");
			}

			byte[] oneConfigBytes = configBytes[configType.Name];

			object category = ProtobufHelper.FromBytes(configType, oneConfigBytes, 0, oneConfigBytes.Length);

			AllConfig[configType] = category;

			//lock (self)
			//{
			//    self.AllConfig[configType] = category;
			//}
		}
	}
}