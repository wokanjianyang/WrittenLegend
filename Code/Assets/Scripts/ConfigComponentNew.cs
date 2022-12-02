using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using System.IO;

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

			String[] bundles = Define.GetAssetPathsFromAssetBundle("config.unity3d");
			Log.Info($"bundles-size:{bundles.Length}\n");

			foreach (string path in bundles)
			{
				Log.Info($"bundle:{path}\n");

				string assetName = Path.GetFileNameWithoutExtension(path);
				UnityEngine.Object resource = Define.LoadAssetAtPath(path);
				keys.Add(assetName, resource);
			}

			foreach (var kv in keys)
			{
				TextAsset v = kv.Value as TextAsset;
				string key = kv.Key;
				configBytes[key] = v.bytes;
			}

			foreach (Type type in configTypes)
			{
				LoadOneInThread(type, configBytes);
			}
		}

		private static void LoadOneInThread(Type configType, Dictionary<string, byte[]> configBytes)
		{
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