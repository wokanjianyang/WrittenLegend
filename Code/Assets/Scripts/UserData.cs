using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using SA.Android.Utilities;
using System.Linq;
using Game.Data;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Text;

namespace Game
{
    public class UserData
    {
        static string savePath = "user";
        static string fileName = "data.json"; //文件名
        static string tempName = "temp.json"; //文件名

        public static long StartTime = 0;
        //public static string tapAccount = "";

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
            }

            if (user.EquipPanelList.Count < 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (!user.EquipPanelList.ContainsKey(i))
                    {
                        user.EquipPanelList[i] = new Dictionary<int, Equip>();
                    }
                }
            }

            if (user.ExclusivePanelList.Count < 7)
            {
                for (int i = 0; i < 7; i++)
                {
                    if (!user.ExclusivePanelList.ContainsKey(i))
                    {
                        user.ExclusivePanelList[i] = new Dictionary<int, ExclusiveItem>();
                    }
                }
            }

            if (user.MagicLevel.Data <= 0)
            {
                user.MagicLevel.Data = 1;
            }

            if (user.DefendData == null)
            {
                user.DefendData = new DefendData();
            }
            if (!user.DefendData.CountDict.ContainsKey(1))
            {
                MagicData data = new MagicData();
                data.Data = 1;
                user.DefendData.CountDict[1] = data;
            }
            if (!user.DefendData.CountDict.ContainsKey(2))
            {
                MagicData data = new MagicData();
                data.Data = 1;
                user.DefendData.CountDict[2] = data;
            }

            if (user.HeroPhatomData == null)
            {
                user.HeroPhatomData = new HeroPhatomData();
                user.HeroPhatomData.Count.Data = 1;
            }

            if (user.DeviceId == "")
            {
                user.DeviceId = AppHelper.GetDeviceIdentifier();
            }

            if (user.MinerList.Count <= 0)
            {

                Miner miner = new Miner();
                miner.Init("矿工1");
                user.MinerList.Add(miner);
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    user.MinerList[0].InlineBuild();
                }
            }

            //去掉专属精华
            //user.Bags.RemoveAll(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie);

            //user.DefendData.Refresh();
            //user.DefendData.CountDict[1].Data = 10;
            //user.HeroPhatomData = new HeroPhatomData();
            //user.HeroPhatomData.Count.Data = 1;
            //TEST data
            //user.MagicGold.Data = 200000000000000000; 
            //user.Level = 1;
            //user.MapId = 1010;
            //user.TowerFloor = 59998;
            //user.PhantomRecord.Clear();
            //user.Exp = 999999999999;
            //TestFull(user);

            //记录版号
            user.VersionLog[ConfigHelper.Version] = TimeHelper.ClientNowSeconds();

            return user;
        }

        public static void Save(bool andTemp = false)
        {
            if (GameProcessor.Inst == null || GameProcessor.Inst.User == null)
            {
                return;
            }
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
            SaveData(str_json, andTemp);
        }

        public static void SaveData(string str_json, bool andTemp)
        {
            string filePath = getSavePath();             //文件路径

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 写入要保存的内容
                writer.Write(str_json);
            }

            if (andTemp)
            {
                string tempPath = getTempPath();
                using (StreamWriter writer = new StreamWriter(tempPath))
                {
                    // 写入要保存的内容
                    writer.Write(str_json);
                }
            }
        }

        public static string getBackupPath()
        {
            // 构建要读取的文件路径
            string folderPath = Path.Combine(Application.persistentDataPath + "/../..", "fulljoblegend");

            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);

            }

            string filePath = Path.Combine(folderPath, fileName);             //文件路径

            if (!File.Exists(filePath))
            {
                //创建文件
                File.Create(filePath).Dispose();
            }

            return filePath;
        }

        public static string getSavePath()
        {
            string folderPath = Path.Combine(Application.persistentDataPath, savePath); //文件夹路径

            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);             //文件路径

            if (!File.Exists(filePath))
            {
                //创建文件
                File.Create(filePath).Dispose();
            }

            return filePath;
        }

        public static string getTempPath()
        {
            string folderPath = Path.Combine(Application.persistentDataPath, savePath); //文件夹路径

            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, tempName);             //文件路径

            if (!File.Exists(filePath))
            {
                //创建文件
                File.Create(filePath).Dispose();
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

        //public static void TestFull(User user)
        //{
        //    user.MagicLevel.Data = ConfigHelper.Max_Level;

        //    //精练
        //    foreach (var kv in user.MagicEquipRefine)
        //    {
        //        kv.Value.Data = ConfigHelper.Max_Level_Refine;
        //    }

        //    //强化
        //    foreach (var kv in user.MagicEquipStrength)
        //    {
        //        kv.Value.Data = ConfigHelper.Max_Level;
        //    }

        //    //图鉴
        //    foreach (var kv in user.CardData)
        //    {
        //        try
        //        {
        //            CardConfig cardConfig = CardConfigCategory.Instance.Get(kv.Key);
        //            kv.Value.Data = cardConfig.MaxLevel;
        //        }
        //        catch { }
        //    }

        //    //魂环
        //    foreach (var kv in user.SoulRingData)
        //    {
        //    }

        //    //幻神
        //    List<PhantomConfig> phantoms = PhantomConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
        //    foreach (var kv in phantoms)
        //    {
        //        user.PhantomRecord[kv.Id] = 15;
        //    }

        //    //广告
        //    user.Record.AddRecord(RecordType.AdVirtual, 1000);
        //    user.Record.AddRecord(RecordType.AdReal, 1000);

        //    user.MagicRecord[AchievementSourceType.BossFamily].Data = 10000;

        //    //技能
        //}
    }
}