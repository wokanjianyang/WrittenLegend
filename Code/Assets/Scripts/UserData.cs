using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class UserData
    {
        static Hero hero;
        static string savePath = "user";
        static string fileName = "data.json"; //文件名
        public static Hero Load() {
            if(hero==null)
            {
                string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径                                        
                string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径
                Debug.Log($"存档路径：{filePath}");

                if (System.IO.File.Exists(filePath))
                {
                    //读取文件
                    System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                    string str_json = sr.ReadToEnd();
                    sr.Close();
                    //反序列化
                    hero = JsonConvert.DeserializeObject<Hero>(str_json);

                    if (hero == null) {
                        hero = new Hero();
                        //首次初始化
                        hero.Level = 1;
                        hero.Exp = 0;
                        hero.Name = "传奇";
                    }

                    Debug.Log("成功读取");
                }
                else {
                    hero = new Hero();
                    //首次初始化属性
                    hero.Level = 1;
                    hero.Exp = 0;
                    hero.Name = "传奇";
                    Save();
                }
            }
            return hero;
        }

        public static void Save() {
            //
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //文件夹路径
            System.IO.Directory.CreateDirectory(folderPath);

            //创建一个空白文件
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //文件路径
            System.IO.File.Create(filePath).Dispose();

            //序列化
            string str_json = JsonConvert.SerializeObject(hero);

            //写入文件
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath);
            sw.Write(str_json);
            sw.Close();

            //确认保存
            if (System.IO.File.Exists(filePath))
                Debug.Log("保存成功");
            else
                Debug.Log("保存失败");
        }
    }
}
