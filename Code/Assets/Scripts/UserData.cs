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
        static string fileName = "data.json"; //�ļ���
        public static Hero Load() {
            Hero hero = null;
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //�ļ���·��                                        
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //�ļ�·��
            Debug.Log($"�浵·����{filePath}");

            if (System.IO.File.Exists(filePath))
            {
                //��ȡ�ļ�
                System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                string str_json = sr.ReadToEnd();
                sr.Close();
                //�����л�
                hero = JsonConvert.DeserializeObject<Hero>(str_json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                if (hero == null) {
                    hero = new Hero();
                    //�״γ�ʼ��
                    hero.Level = 1;
                    hero.Exp = 0;
                    hero.Name = "����";
                }

                Debug.Log("�ɹ���ȡ");
            }
            else {
                hero = new Hero();
                //�״γ�ʼ������
                hero.Level = 1;
                hero.Exp = 0;
                hero.Name = "����";
                Save();
            }
            return hero;
        }

        public static void Save()
        {
            //
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //�ļ���·��
            System.IO.Directory.CreateDirectory(folderPath);

            //����һ���հ��ļ�
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //�ļ�·��
            System.IO.File.Create(filePath).Dispose();

            System.TimeSpan st = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0);//��ȡʱ���
            //Log.Debug($"����ʱ��:{DateTime.UtcNow.ToString("F")}");

            var hero = GameProcessor.Inst.PlayerManager.GetHero();
            hero.LastOut = Convert.ToInt64(st.TotalSeconds);

            //���л�
            string str_json = JsonConvert.SerializeObject(hero, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            if (str_json.Length <= 0)
            {
                return;
            }

            //д���ļ�
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath);
            sw.Write(str_json);
            sw.Close();

            //ȷ�ϱ���
            if (System.IO.File.Exists(filePath))
            {
                //Log.Debug("����ɹ�");
            }
            else
                Log.Debug("����ʧ��");
        }

        public static void Delete()
        {
            string folderPath = System.IO.Path.Combine(Application.persistentDataPath, savePath); //�ļ���·��
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //�ļ�·��
            System.IO.File.Delete(filePath);
        }
    }
}
