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
        public Text Txt_Save;
        public Button btn_Load;
        public Text Txt_Load;

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
                this.txt_Info.gameObject.SetActive(false);
                this.txt_Memo.gameObject.SetActive(false);

                this.btn_Save.gameObject.SetActive(false);
                this.btn_Load.gameObject.SetActive(false);
            }
            else
            {
                Show();
            }
        }

        float currentRoundTime = 0;
        private void Update()
        {
            if (ConfigHelper.Channel != ConfigHelper.Channel_Tap)
            {
                this.currentRoundTime += Time.unscaledDeltaTime;
                if (this.currentRoundTime >= 1)
                {
                    Show();
                }
            }
        }

        private void Show()
        {
            User user = GameProcessor.Inst.User;

            long now = TimeHelper.ClientNowSeconds();
            long cdSaveTime = now - user.SaveTicketTime;
            cdSaveTime = 3700;
            if (cdSaveTime > 3600)
            {
                btn_Save.gameObject.SetActive(true);
                Txt_Save.gameObject.SetActive(false);
            }
            else
            {
                btn_Save.gameObject.SetActive(false);
                Txt_Save.gameObject.SetActive(true);
                Txt_Save.text = TimeSpan.FromSeconds(3600 - cdSaveTime).ToString(@"hh\:mm\:ss");
            }

            long cdLoadTime = now - user.LoadTicketTime;
            cdLoadTime = 3700;
            if (cdLoadTime > 3600)
            {
                btn_Load.gameObject.SetActive(true);
                Txt_Load.gameObject.SetActive(false);
            }
            else
            {
                btn_Load.gameObject.SetActive(false);
                Txt_Load.gameObject.SetActive(true);
                Txt_Load.text = TimeSpan.FromSeconds(3600 - cdSaveTime).ToString(@"hh\:mm\:ss");
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

        private void saveData()
        {
            User user = GameProcessor.Inst.User;
            user.SaveTicketTime = TimeHelper.ClientNowSeconds();
            btn_Save.gameObject.SetActive(false);

            this.txt_Info.text = "存档中......";

            try
            {
                string str_json = JsonConvert.SerializeObject(user, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                str_json = EncryptionHelper.AesEncrypt(str_json);

                string md5 = EncryptionHelper.Md5(str_json);
                Debug.Log("save md5:" + md5);
                byte[] bytes = Encoding.UTF8.GetBytes(str_json);

                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("md5", md5);

                //再存储新档
                StartCoroutine(NetworkHelper.UploadData(bytes, headers,
                        (WebResultWrapper result) =>
                        {
                            if (result.Code == StatusMessage.OK)
                            {
                                this.txt_Info.text = "存档成功.";
                            }
                            else
                            {
                                this.txt_Info.text = "存档失败.";
                                user.SaveTicketTime = TimeHelper.ClientNowSeconds() - 3600;
                            }
                        },
                        () =>
                        {
                            btn_Load.gameObject.SetActive(true);
                            this.txt_Info.text = "存档失败.";
                            user.SaveTicketTime = TimeHelper.ClientNowSeconds() - 3600;
                        }
                        ));
            }
            catch (Exception ex)
            {
                this.txt_Info.text = "存档失败，请稍等一会重试...";
            }
        }


        private void loadData()
        {
            User user = GameProcessor.Inst.User;
            user.LoadTicketTime = TimeHelper.ClientNowSeconds();

            btn_Load.gameObject.SetActive(false);

            this.txt_Info.text = "读档中......";

            try
            {

                StartCoroutine(NetworkHelper.DownData(
                  (byte[] bytes) =>
                  {
                      Time.timeScale = 0;

                      if (bytes == null)
                      {
                          this.txt_Info.text = "读档失败,还没有存档或者其他错误.";
                          return;
                      }

                      string str_json = Encoding.UTF8.GetString(bytes);
                      string md5 = EncryptionHelper.Md5(str_json);
                      Debug.Log("load md5:" + md5);
                      str_json = EncryptionHelper.AesDecrypt(str_json);

                      if (GameProcessor.Inst.LoadInit(str_json))
                      {
                          this.txt_Info.text = "读取存档成功,请退出重进";
                          UserData.Save();
                      }
                      else
                      {
                          this.txt_Info.text = "读取失败,存档损坏,取消读档,请退出重进";
                      }
                      Application.Quit();
                  },
                  () =>
                  {
                      btn_Load.gameObject.SetActive(true);
                      user.LoadTicketTime = TimeHelper.ClientNowSeconds() - 3600;
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
