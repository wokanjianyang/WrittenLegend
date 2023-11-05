using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using SA.Android.Utilities;
using System.Linq;
using Game.Data;
using System.IO;
using TapTap.Bootstrap;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Text;

namespace Game
{
    public class UserData
    {
        static string savePath = "user";
        static string fileName = "data.json"; //文件名

        public static long StartTime = 0;
        public static string pn = "";
        public static string sk = "";

        public static User Load()
        {
            User user = null;

            string filePath = getSavePath();
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
                //Debug.Log("成功读取");

                if (user != null && user.VersionLog.Count <= 0)
                {
                    //老版本,直接销存档
                    //user = null;
                }
            }

            if (user == null)
            {
                user = new User();
                //首次初始化
                user.MagicLevel.Data = 1;
                user.MagicExp.Data = 0;
                user.Name = "传奇";
                user.MagicTowerFloor.Data = 1;
                user.MapId = ConfigHelper.MapStartId;
                user.MagicGold.Data = 0;
                user.MagicCopyTikerCount.Data = ConfigHelper.CopyTicketFirstCount;
                user.SaveLimit = 5;
                user.LoadLimit = 5;

                user.EquipPanelList[0] = new Dictionary<int, Equip>();
                user.EquipPanelList[1] = new Dictionary<int, Equip>();
                user.EquipPanelList[2] = new Dictionary<int, Equip>();
                user.EquipPanelList[3] = new Dictionary<int, Equip>();
                user.EquipPanelList[4] = new Dictionary<int, Equip>();
            }
            else
            {
                if (user.packName != "" && user.packName != "3241c82c420823c129660e367cb91c60")
                {
                    return null;
                }

                if (user.signKey != "" && user.signKey != "312fc4ca3769fe53c60c234371f89a6f")
                {
                    return null;
                }
            }

            if (user.MagicLevel.Data <= 0)
            {
                user.MagicLevel.Data = 1;
            }

            if (user.DefendData == null)
            {
                user.DefendData = new DefendData();
                user.DefendData.Count.Data = 1;
            }

            //TEST data
            //user.MagicGold.Data = 9999999999999; 
            //user.Level = 1;
            //user.MapId = 1010;
            //user.TowerFloor = 59998;
            //user.PhantomRecord.Clear();
            //user.Exp = 999999999999;

            //记录版号
            user.VersionLog[ConfigHelper.Version] = TimeHelper.ClientNowSeconds();

            return user;
        }

        public static void Save()
        {
            if (GameProcessor.Inst == null)
            {
                return;
            }
            var user = GameProcessor.Inst.User;
            user.LastOut = TimeHelper.ClientNowSeconds();
            user.packName = UserData.pn;
            user.signKey = UserData.sk;

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
            SaveData(str_json);
        }

        public static void SaveData(string str_json)
        {
            string filePath = getSavePath();             //文件路径

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 写入要保存的内容
                writer.Write(str_json);
            }
        }

        public static string getSavePath()
        {
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径

            if (!File.Exists(folderPath))
            {
                System.IO.Directory.CreateDirectory(folderPath);
            }

            string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径

            if (!File.Exists(filePath))
            {
                //创建文件
                System.IO.File.Create(filePath).Dispose();
            }

            return filePath;
        }

        public static void Clear()
        {
            string str_json = "";

            //创建一个空白文件
            string filePath = getSavePath();             //文件路径

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 写入要保存的内容
                writer.Write(str_json);
            }
        }
    }
}