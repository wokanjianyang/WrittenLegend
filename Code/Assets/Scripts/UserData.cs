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
            //Debug.Log($"存档路径：{filePath}");

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

                user.EquipPanelList[0] = new Dictionary<int, Equip>();
                user.EquipPanelList[1] = new Dictionary<int, Equip>();
                user.EquipPanelList[2] = new Dictionary<int, Equip>();
                user.EquipPanelList[3] = new Dictionary<int, Equip>();
                user.EquipPanelList[4] = new Dictionary<int, Equip>();
            }

            if (user.MagicLevel.Data <= 0)
            {
                //user.MagicLevel.Data = 1;
            }

            //clear month
            user.Bags.RemoveAll(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Moon_Cake);

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

        public static async Task<User> LoadTapData()
        {
            Debug.Log("LoadTapData Start");

            User user = null;

            var collection = await TapGameSave.GetCurrentUserGameSaves();


            Debug.Log("LoadTapData Count:" + collection.Count());

            foreach (var gameSave in collection)
            {
                Debug.Log("LoadTapData:" + gameSave.ToString());

                var summary = gameSave.Summary;
                var modifiedAt = gameSave.ModifiedAt;
                var playedTime = gameSave.PlayedTime;
                var progressValue = gameSave.ProgressValue;
                var coverFile = gameSave.Cover;
                var gameFile = gameSave.GameFile;
                var gameFileUrl = gameFile.Url;
            }

            Debug.Log("LoadTapData Over");

            return user;
        }

        public static void Save()
        {
            var user = GameProcessor.Inst.User;
            user.LastOut = TimeHelper.ClientNowSeconds();

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

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 写入要保存的内容
                writer.Write(str_json);
            }
        }

        public static async Task SaveTapData()
        {

            Debug.Log("SaveTapData Start");

            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径                                        
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径

            var gameSave = new TapGameSave
            {
                Name = "UserData",
                Summary = "description",
                ModifiedAt = DateTime.Now.ToLocalTime(),
                PlayedTime = 60000L, // ms
                ProgressValue = 100,
                //CoverFilePath = "", // jpg/png
                GameFilePath = filePath,
            };

            var res = gameSave.Save();

            Debug.Log("SaveTapData Result:" + res.ToString());

            Debug.Log("SaveTapData Over");
        }

        public static async Task CearTapData() { 

        }

        public static void Clear()
        {
            string str_json = "";
            //
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径
            System.IO.Directory.CreateDirectory(folderPath);

            //创建一个空白文件
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 写入要保存的内容
                writer.Write(str_json);
            }
        }
    }
}