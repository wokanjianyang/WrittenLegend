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
using Game.Data;

namespace Game
{
    public class Com_Other : MonoBehaviour
    {
        [LabelText("显示怪物技能特效")]
        public Toggle tog_Monster_Skill;

        //public Button btn_Query;

        public Text txt_Info;
        public Text txt_Memo;

        public Text Txt_DeviceId;
        public Text Txt_FileId;

        public Transform Tf_Login;
        public InputField If_Account;
        public InputField If_Pwd;
        public Button btn_Change;

        public Button btn_Save;
        public Button btn_Load;

        // Start is called before the first frame update
        void Start()
        {
            tog_Monster_Skill.onValueChanged.AddListener((isOn) =>
            {
                this.ShowSkill(isOn);
            });

            this.btn_Change.onClick.AddListener(this.OnClick_Change);
            this.btn_Save.onClick.AddListener(this.OnClick_Save);
            this.btn_Load.onClick.AddListener(this.OnClick_Load);

            //this.CheckProgress();

            //AsyncLoginTap();
            if (ConfigHelper.Channel == ConfigHelper.Channel_Tap)
            {
                this.Tf_Login.gameObject.SetActive(false);
                this.btn_Save.gameObject.SetActive(false);
                this.btn_Load.gameObject.SetActive(false);
                this.txt_Info.gameObject.SetActive(false);
                this.txt_Memo.gameObject.SetActive(false);
            }

        }

        public void Init()
        {
            tog_Monster_Skill.isOn = GameProcessor.Inst.User.ShowMonsterSkill;

            User user = GameProcessor.Inst.User;

            string account = user.Account;
            //this.txt_Account.text = "设备Id:" + id;

            if (account == "")
            {
                this.Tf_Login.gameObject.SetActive(true);

                this.btn_Save.gameObject.SetActive(false);
                this.btn_Load.gameObject.SetActive(false);
                this.txt_Memo.gameObject.SetActive(false);
            }
            else
            {
                this.Tf_Login.gameObject.SetActive(false);

                this.btn_Save.gameObject.SetActive(true);
                this.btn_Load.gameObject.SetActive(true);
                this.txt_Memo.gameObject.SetActive(true);
                this.txt_Memo.text = "您已经绑定了存档,您的存档帐号为:" + account + ".\n"
                    + "如果您需要切换设备，则在新设备输入同样的帐号和密码,\n"
                    + "再点击绑定，新设备就可以读取存档了。\n"
                    + "请不要一个存档绑定太多设备，会导致封号无法使用云存档。\n";
            }

            this.Txt_FileId.text = "存档Id:" + user.DeviceId;
            this.Txt_DeviceId.text = "设备Id:" + AppHelper.GetDeviceIdentifier();
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
            string account = If_Account.text;
            if (account.Length < 6 || account.Length > 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "帐号请输入6-10个字符", ToastType = ToastTypeEnum.Failure });
                return;
            }

            string pwd = If_Pwd.text;
            if (pwd.Length < 6 || pwd.Length > 10)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "密码请输入6-10个字符", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.btn_Change.gameObject.SetActive(false);
            this.txt_Info.text = "绑定中...";

            StartCoroutine(NetworkHelper.CreateAccount(account, pwd,
                 (WebResultWrapper result) =>
                 {
                     if (result.Code == StatusMessage.OK)
                     {
                         GameProcessor.Inst.User.Account = account;
                         UserData.Save();

                         this.Tf_Login.gameObject.SetActive(false);

                         this.btn_Save.gameObject.SetActive(true);
                         this.btn_Load.gameObject.SetActive(true);
                         this.txt_Memo.gameObject.SetActive(true);
                         this.txt_Memo.text = "您已经绑定了存档,您的存档帐号为:" + account + ".\n"
                           + "如果您需要切换设备，则在新设备输入同样的帐号和密码,\n"
                           + "再点击绑定，新设备就可以读取存档了。\n"
                           + "请不要一个存档绑定太多设备，会导致封号无法使用云存档。\n";
                     }
                     else
                     {
                         this.btn_Change.gameObject.SetActive(true);
                     }

                     this.txt_Info.text = result.Msg;
                 },
                 () =>
                 {
                     this.btn_Change.gameObject.SetActive(true);
                     this.txt_Info.text = "网络错误.";
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

            //txt_Desc.text = "今天剩余存档次数:" + user.SaveLimit + ",读档次数:" + user.LoadLimit;

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
                this.txt_Info.text = "今天存档次数已满";
                return;
            }

            this.txt_Info.text = "存档中......";

            try
            {

                string filePath = UserData.getSavePath();
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);

                //再存储新档
                StartCoroutine(NetworkHelper.UploadData(bytes,
                        (WebResultWrapper result) =>
                        {
                            if (result.Code == StatusMessage.OK)
                            {
                                this.txt_Info.text = "存档成功.";
                            }
                            else
                            {
                                this.txt_Info.text = "存档失败.";
                            }

                            btn_Load.gameObject.SetActive(true);
                        },
                        () =>
                        {
                            btn_Load.gameObject.SetActive(true);
                            this.txt_Info.text = "存档失败.";
                        }
                        ));
            }
            catch (Exception ex)
            {
                this.txt_Info.text = "存档失败，请稍等一会重试...";
            }

            this.CheckProgress();
        }


        private void loadData()
        {
            User user = GameProcessor.Inst.User;
            btn_Load.gameObject.SetActive(false);

            this.txt_Info.text = "读档中......";

            try
            {
                //再存储新档
                StartCoroutine(NetworkHelper.DownData(
                        (byte[] bytes) =>
                        {
                            Time.timeScale = 0;

                            if (bytes == null)
                            {
                                this.txt_Info.text = "读档失败,还没有存档或者其他错误.";
                                return;
                            }

                            Debug.Log("bytes:" + bytes.Length);

                            string str_json = Encoding.UTF8.GetString(bytes);
                            Debug.Log(Encoding.UTF8.GetString(bytes));


                            this.txt_Info.text = "读取存档成功,请退出重进";
                            UserData.SaveData(str_json, false);

                            GameProcessor.Inst.Init(UserData.StartTime);

                            GameProcessor.Inst.User.DataDate = DateTime.Now.Ticks;
                            UserData.Save();

                            Application.Quit();
                        },
                        () =>
                        {
                            btn_Load.gameObject.SetActive(true);
                            this.txt_Info.text = "读档失败.";
                        }
                        ));
            }
            catch (Exception ex)
            {
                this.txt_Info.text = "读档失败，请稍等一会重试...";
            }
        }
    }
}
