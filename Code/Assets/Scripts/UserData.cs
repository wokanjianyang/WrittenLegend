using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using SA.Android.Utilities;

namespace Game
{
    public class UserData
    {
        static string savePath = "user";
        static string fileName = "data.json"; //文件名

        public static long StartTime = 0;
        public static User Load()
        {
            User user = null;
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径                                        
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径
            Debug.Log($"存档路径：{filePath}");

            if (System.IO.File.Exists(filePath))
            {
                //读取文件
                System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                string str_json = sr.ReadToEnd();
                sr.Close();

                str_json = EncryptionHelper.AesDecrypt(str_json);

                //反序列化
                user = JsonConvert.DeserializeObject<User>(str_json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                Debug.Log("成功读取");
            }

            if (user == null)
            {
                user = new User();
                //首次初始化
                user.Level = 1;
                user.Exp = 0;
                user.Name = "传奇";
                user.TowerFloor = 1;
                user.MapId = 1000;
                user.Gold = 0;
            }
            //TEST data
            //user.Gold = 999999999999; 
            //user.Level = 1000000;
            //user.MapId = 1010;
            //user.TowerFloor = 989;
            //user.PhantomRecord.Clear();
            return user;
        }

        public static void Save()
        {
            System.TimeSpan st = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0);//获取时间戳
            //Log.Debug($"离线时间:{DateTime.UtcNow.ToString("F")}");

            var user = GameProcessor.Inst.User;
            user.LastOut = Convert.ToInt64(st.TotalSeconds);

            //序列化
            string str_json = JsonConvert.SerializeObject(user, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (str_json.Length <= 0)
            {
                return;
            }

            //加密
            str_json = EncryptionHelper.AesEncrypt(str_json);

            //
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径
            System.IO.Directory.CreateDirectory(folderPath);

            //创建一个空白文件
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径
            System.IO.File.Create(filePath).Dispose();

            //写入文件
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath);
            sw.Write(str_json);
            sw.Close();

            //确认保存
            if (System.IO.File.Exists(filePath))
            {
                Log.Debug("保存成功");
            }
            else
                Log.Debug("保存失败");
        }

        public static void Delete()
        {
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径
            System.IO.File.Delete(filePath);
        }
    }
}