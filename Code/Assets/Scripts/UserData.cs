using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class UserData
    {

        static string savePath = "user";
        static string fileName = "data.json"; //�ļ���
        public static void Load(out Hero hero) {
            string folderPath = System.IO.Path.Combine(Application.dataPath, savePath); //�ļ���·��                                        
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //�ļ�·��

            if (System.IO.File.Exists(filePath))
            {
                //��ȡ�ļ�
                System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                string str_json = sr.ReadToEnd();
                sr.Close();
                //�����л�
                hero = JsonConvert.DeserializeObject<Hero>(str_json);

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
            }
        }

        public static void Save(Hero hero) {
            //
            string folderPath = System.IO.Path.Combine(Application.dataPath, savePath); //�ļ���·��
            System.IO.Directory.CreateDirectory(folderPath);

            //����һ���հ��ļ�
            string filePath = System.IO.Path.Combine(folderPath, fileName);             //�ļ�·��
            System.IO.File.Create(filePath).Dispose();

            //���л�
            string str_json = JsonConvert.SerializeObject(hero);

            //д���ļ�
            System.IO.StreamWriter sw = new System.IO.StreamWriter(filePath);
            sw.Write(str_json);
            sw.Close();

            //ȷ�ϱ���
            if (System.IO.File.Exists(filePath))
                Debug.Log("����ɹ�");
            else
                Debug.Log("����ʧ��");
        }
    }
}
