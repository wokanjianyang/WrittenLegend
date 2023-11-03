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
			if (md5 != "9CE9AA3B9097F2FD31945C1FE070C8AE")
			{
#if !UNITY_EDITOR
				return;
#endif
			}



			foreach (Type type in configTypes)
			{
				LoadOneInThread(type, configBytes);
			}
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