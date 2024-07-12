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
        static string ppKey = "key";

        static string[] BackKeyList = { "key1", "key2", "key3" };
        static string backPath = "back";
        static string[] BackFileName = { "back1.json", "back2.json", "back3.json" };

        public static long StartTime = 0;
        //public static string tapAccount = "";


        public static string GetBackKey(int index)
        {
            return BackKeyList[index] + "" + ConfigHelper.Channel;
        }

        public static User LoadByKey(int index)
        {
            Debug.Log("读取备份文件:" + index);

            string filePath = GetBackPath(index);
            //Debug.Log("备份路径:" + index + " " + filePath);
            string backKey = GetBackKey(index);
            string key = PlayerPrefs.GetString(backKey);
            //Debug.Log("备份Key:" + index + " " + key);

            if (key == "")
            {
                return null;
            }

            //读取文件
            StreamReader sr = new StreamReader(filePath);
            string str_json = sr.ReadToEnd();
            sr.Close();

            if (str_json.Length <= 0)
            {
                return null;
            }
            //Debug.Log("备份str_json:" + index + " " + str_json);

            str_json = EncryptionHelper.AesDecrypt(str_json, key);

            //Debug.Log("备份json:" + index + " " + str_json);

            User user = JsonConvert.DeserializeObject<User>(str_json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            return user;
        }

        public static User Load()
        {
            User user = null;

            string filePath = GetSavePath();
            //Debug.Log($"存档路径：{filePath}");

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    //PlayerPrefs.DeleteAll();
                    string key = PlayerPrefs.GetString(ppKey);

                    //读取文件
                    System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                    string str_json = sr.ReadToEnd();
                    sr.Close();

                    if (str_json.Length > 0)
                    {
                        if (key == "" && ConfigHelper.Version <= 209)
                        {
                            str_json = EncryptionHelper.AesDecrypt(str_json);
                        }
                        else
                        {
                            str_json = EncryptionHelper.AesDecrypt(str_json, key);
                        }

                        user = JsonConvert.DeserializeObject<User>(str_json, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        });
                        //Debug.Log("成功读取");
                    }

                    if (user == null)
                    {
                        for (int i = 0; i < BackKeyList.Length; i++)
                        {
                            user = LoadByKey(i);
                            if (user != null)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            try
            {
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
                    user.FirstTime = TimeHelper.ClientNowSeconds();
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

                if (user.InfiniteData == null)
                {
                    user.InfiniteData = new InfiniteData();
                }

                if (user.LegacyData == null)
                {
                    user.LegacyData = new LegacyData();
                    user.LegacyData.GetDropId(1);
                    user.LegacyData.GetDropId(2);
                    user.LegacyData.GetDropId(3);
                    user.LegacyData.GetDropLayer(1, 1);
                    user.LegacyData.GetDropLayer(2, 1);
                    user.LegacyData.GetDropLayer(3, 1);
                }

                if (user.DeviceId == "")
                {
                    user.DeviceId = AppHelper.GetDeviceIdentifier();
                }

                if (user.RecoverySetting.SkillReserveQuanlity.Count() == 0)
                {
                    user.RecoverySetting.SkillReserveQuanlity[4] = true;
                    user.RecoverySetting.SkillReserveQuanlity[5] = true;
                }

                List<BoxItem> cfList = user.Bags.Where(m => m.Item.Type == ItemType.Card || m.Item.Type == ItemType.Fashion
                || (m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Card_Stone)).ToList();
                if (cfList.Count > 0)
                {
                    foreach (BoxItem cf in cfList)
                    {
                        user.SaveItemMeterialCount(cf.Item.ConfigId, cf.MagicNubmer.Data);
                    }

                    user.Bags.RemoveAll(m => m.Item.Type == ItemType.Card || m.Item.Type == ItemType.Fashion || (m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Card_Stone));
                }

                //user.KillRecord.Clear();

                //user.InfiniteData = new InfiniteData();
                //user.InfiniteData.Ticket = 0;
                //Debug.Log("DeviceId:" + user.DeviceId);

                //去掉专属精华
                //user.Bags.RemoveAll(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Chunjie);

                //List<BoxItem> items = user.Bags.Where(m => m.Item.Type == ItemType.Equip && m.Item.ConfigId >= 21105801 && m.Item.ConfigId <= 21105810).ToList();
                //List<BoxItem> items = user.Bags.Where(m => m.Item.Type == ItemType.Equip && m.Item.ConfigId >= 22105801 && m.Item.ConfigId <= 22105810).ToList();
                //List<BoxItem> items = user.Bags.Where(m => m.Item.Type == ItemType.Equip && m.Item.ConfigId >= 23105801 && m.Item.ConfigId <= 23105810).ToList();
                //Debug.Log("items:" + items.Count);
                //foreach (var item in items) {
                //    Equip equip = item.Item as Equip;
                //    equip.ConfigId -= 1000000;
                //}

                //user.Bags.RemoveAll(m => m.Item.Type != 0);

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
                //user.AdData.CodeDict.Clear();

                //补偿
                //user.MagicLevel.Data = 30000;
                //user.Record.AddRecord(RecordType.AdReal, 360);

                //超出上限的技能
                foreach (SkillData skill in user.SkillList)
                {
                    int lm = user.GetSkillLimit(skill.SkillConfig);
                    if (skill.MagicLevel.Data > lm)
                    {
                        skill.MagicLevel.Data = lm;
                    }
                }

                //记录版号
                user.VersionLog[ConfigHelper.Version] = TimeHelper.ClientNowSeconds();
            }
            catch
            {
            }

            return user;
        }

        public static void Save()
        {
            if (GameProcessor.Inst == null || GameProcessor.Inst.User == null)
            {
                return;
            }
            var user = GameProcessor.Inst.User;
            //user.LastOut = TimeHelper.ClientNowSeconds();

            //序列化
            string str_json = JsonConvert.SerializeObject(user, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (str_json.Length <= 0)
            {
                return;
            }

            string key = Guid.NewGuid().ToString().Substring(0, 16);
            //Debug.Log("save key" + key);


            //加密
            str_json = EncryptionHelper.AesEncrypt(str_json, key);

            string filePath = GetSavePath();             //文件路径

            try
            {
                File.WriteAllText(filePath, str_json);
                PlayerPrefs.SetString(ppKey, key);
                PlayerPrefs.Save();

                Debug.Log("saved successfully.");
            }
            catch (Exception ex)
            {

                Debug.Log("saved Error." + ex.Message);
            }

            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    // 写入要保存的内容
            //    writer.Write(str_json);
            //}
        }

        public static void SaveBack(int index)
        {
            if (GameProcessor.Inst == null || GameProcessor.Inst.User == null)
            {
                return;
            }
            var user = GameProcessor.Inst.User;
            //user.LastOut = TimeHelper.ClientNowSeconds();

            //序列化
            string str_json = JsonConvert.SerializeObject(user, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (str_json.Length <= 0)
            {
                return;
            }

            //Debug.Log("save back " + index);

            string pk = GetBackKey(index);
            string pv = Guid.NewGuid().ToString().Substring(0, 16);
            //Debug.Log("back pp: " + pk + " " + pv);

            //加密
            str_json = EncryptionHelper.AesEncrypt(str_json, pv);

            string filePath = GetBackPath(index);             //文件路径

            try
            {
                File.WriteAllText(filePath, str_json);
                PlayerPrefs.SetString(pk, pv);
                PlayerPrefs.Save();

                Debug.Log("save back successfully.");
            }
            catch (Exception ex)
            {
                Debug.Log("saved back Error." + ex.Message);
            }
            //Debug.Log("back filePath: " + filePath);
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    // 写入要保存的内容
            //    writer.Write(str_json);
            //}
        }

        public static string GetSavePath()
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

        public static string GetBackPath(int index)
        {
            string folderPath = Path.Combine(Application.persistentDataPath, backPath); //文件夹路径

            if (!File.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string backFileName = BackFileName[index];
            string filePath = Path.Combine(folderPath, backFileName);             //文件路径

            if (!File.Exists(filePath))
            {
                //创建文件
                File.Create(filePath).Dispose();
            }

            return filePath;
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