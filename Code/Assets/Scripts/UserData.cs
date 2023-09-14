using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using SA.Android.Utilities;
using System.Linq;
using Game.Data;

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

            //装备方案更新
            if (user.EquipPanelList.Count == 3)
            {
                user.EquipPanelList[3] = new Dictionary<int, Equip>();
                user.EquipPanelList[4] = new Dictionary<int, Equip>();
            }

            //删除装备栏的四格
            foreach (var kvp in user.EquipPanelList)
            {
                var ep = kvp.Value;
                ep.Remove(11);
                ep.Remove(12);
                ep.Remove(13);
                ep.Remove(14);
            }

            //等级转换
            if (user.MagicLevel.Data <= 1 && user.Level > 100)
            {
                user.MagicLevel.Data = LevelConfigCategory.ConvertLevel(user.Level, 3200);
                user.Level = 0;

                user.MagicGold.Data = Math.Min(user.Gold, 100000000000); //最多1000亿
                user.Gold = 0;
            }

            if (user.EquipRefine.Count > 0)
            {
                int MaxLevel = user.EquipRefine.Select(m => m.Value).Max();
                int TotalLeve = user.EquipRefine.Select(m => m.Value).Sum();

                foreach (var kv in user.EquipRefine)
                {
                    user.MagicEquipRefine[kv.Key] = new MagicData();
                    user.MagicEquipRefine[kv.Key].Data = Math.Min(110, user.EquipRefine[kv.Key]);
                }

                user.EquipRefine.Clear();

            }

            if (user.EquipStrength.Count > 0)
            {

                foreach (var kv in user.EquipStrength)
                {
                    user.MagicEquipStrength[kv.Key] = new MagicData();
                    user.MagicEquipStrength[kv.Key].Data = LevelConfigCategory.ConvertLevel(user.EquipStrength[kv.Key], 4500);
                }

                user.EquipStrength.Clear();
            }

            if (user.TowerFloor > 100 && user.MagicTowerFloor.Data <= 1)
            {
                user.MagicTowerFloor.Data = user.TowerFloor;
                user.TowerFloor = 0;
            }

            if (user.CopyTikerCount > 0)
            {
                user.MagicCopyTikerCount.Data = user.CopyTikerCount;
                user.CopyTikerCount = 0;
            }

            foreach (var box in user.Bags)
            {
                if (box.Number > 0)
                {
                    box.MagicNubmer.Data = box.Number;
                    box.Number = 0;
                }
            }

            foreach (var skill in user.SkillList)
            {
                int MaxLevel = user.SkillList.Select(m => m.Level).Max();
                int TotalLeve = user.SkillList.Select(m => m.Level).Sum();

                if (skill.Level > 0)
                {
                    skill.MagicLevel.Data = Math.Min(skill.Level, 150);
                    skill.Position = 0;
                    skill.Level = 0;
                }
                if (skill.Exp > 0)
                {
                    skill.MagicExp.Data = Math.Min(skill.Exp, 750);
                    skill.Exp = 0;
                }
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