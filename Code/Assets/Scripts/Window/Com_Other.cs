using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static UnityEngine.UI.Dropdown;
using System;
using SA.Android.App;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.IO;
using Newtonsoft.Json;

namespace Game
{
    public class Com_Other : MonoBehaviour
    {
        [LabelText("显示怪物技能特效")]
        public Toggle tog_Monster_Skill;

        public Button btn_Query;
        public Button btn_Save;
        public Button btn_Load;
        public Text txt_Name;
        public Text txt_Desc;

        public Text Txt_Device;
        public Text Txt_Account;

        public Text Txt_Pwd;
        public InputField If_Pwd;
        public Button btn_Change;


        // Start is called before the first frame update
        void Start()
        {
            tog_Monster_Skill.onValueChanged.AddListener((isOn) =>
            {
                this.ShowSkill(isOn);
            });

            this.btn_Change.onClick.AddListener(this.OnClick_Change);

            //this.btn_Query.onClick.AddListener(this.OnClick_Query);
            this.btn_Save.onClick.AddListener(this.OnClick_Save);
            this.btn_Load.onClick.AddListener(this.OnClick_Load);

            //this.CheckProgress();

            //AsyncLoginTap();


        }

        public void Init()
        {
            tog_Monster_Skill.isOn = GameProcessor.Inst.User.ShowMonsterSkill;

            User user = GameProcessor.Inst.User;

            string key = user.Pwd;
            //this.txt_Account.text = "设备Id:" + id;

            if (key == "")
            {
                this.If_Pwd.gameObject.SetActive(true);
                this.btn_Change.gameObject.SetActive(true);

                this.Txt_Pwd.gameObject.SetActive(false);
            }
            else
            {
                this.If_Pwd.gameObject.SetActive(false);
                this.btn_Change.gameObject.SetActive(false);

                this.Txt_Pwd.gameObject.SetActive(true);
                this.Txt_Pwd.text = "绑定成功,绑定码为:" + key;
            }

            this.Txt_Account.text = "存档Id:" + user.DeviceId;
            this.Txt_Device.text = "设备Id:" + AppHelper.GetDeviceIdentifier();
        }

        public void ShowSkill(bool show)
        {
            GameProcessor.Inst.User.ShowMonsterSkill = show;
        }

        public void Save()
        {

            string filePath = UserData.getBackupPath();

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

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                // 写入要保存的内容
                writer.Write(str_json);
            }
        }

        public void OnClick_Change()
        {
            string key = If_Pwd.text;
            if (key.Length < 6 || key.Length > 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "请输入6-10个字符", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.btn_Change.gameObject.SetActive(false);

            StartCoroutine(NetworkHelper.CreateAccount(key,
                 (string resultText) =>
                 {
                     Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultText);

                     if (result["code"] == "200")
                     {
                         GameProcessor.Inst.User.Pwd = key;
                         UserData.Save();

                         this.If_Pwd.gameObject.SetActive(false);
                         this.Txt_Pwd.text = "绑定成功,绑定码为:" + key;
                     }
                     else
                     {
                         this.txt_Name.text = "绑定失败.";
                     }
                 },
                 () =>
                 {
                     this.btn_Change.gameObject.SetActive(true);
                     GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "网络错误", ToastType = ToastTypeEnum.Failure });
                 }
                 ));
        }

        public void OnClick_Load()
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Hero);

            this.loadData();
        }

        public void OnClick_Save()
        {
            saveData();
        }

        private void CheckProgress()
        {
            User user = GameProcessor.Inst.User;

            txt_Desc.text = "今天剩余存档次数:" + user.SaveLimit + ",读档次数:" + user.LoadLimit;

            if (user.SaveLimit <= 0)
            {
                this.btn_Save.gameObject.SetActive(false);
            }
            else
            {
                this.btn_Save.gameObject.SetActive(true);
            }

            if (user.LoadLimit <= 0)
            {
                this.btn_Load.gameObject.SetActive(false);
            }
            else
            {
                this.btn_Load.gameObject.SetActive(true);
            }
        }

        private void saveData()
        {
            User user = GameProcessor.Inst.User;
            btn_Save.gameObject.SetActive(false);

            if (user.SaveLimit <= 0)
            {
                this.txt_Name.text = "今天存档次数已满";
                return;
            }

            this.txt_Name.text = "存档中......";

            try
            {

                string filePath = UserData.getSavePath();
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);

                //再存储新档
                StartCoroutine(NetworkHelper.UploadData(bytes,
                        (string resultText) =>
                        {
                            Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(resultText);

                            if (result["code"] == "200")
                            {
                                this.txt_Name.text = "存档成功.";
                            }
                            else
                            {
                                this.txt_Name.text = "存档失败.";
                            }
                        },
                        () =>
                        {
                            this.txt_Name.text = "存档失败.";
                        }
                        ));
            }
            catch (Exception ex)
            {
                this.txt_Name.text = "存档失败，请稍等一会重试...";
            }

            this.CheckProgress();
        }

        public static IEnumerator UnityWebRequestPost()
        {
            string url = "http://127.0.0.1:11111/public/save";


            using (UnityWebRequest www = UnityWebRequest.Post(url, "{}"))
            {

                //www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

                string filePath = UserData.getSavePath();
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                www.uploadHandler = new UploadHandlerRaw(fileBytes);
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.LogError($"发起网络请求失败： 确认过闸接口 -{www.error}");
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }

        private void loadData()
        {

        }
    }
}
