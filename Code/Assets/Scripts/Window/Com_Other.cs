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
        public Toggle tog_Monster_Damage;
        //public Button btn_Query;

        public Text txt_Info;
        public Text txt_Memo;

        public Text Txt_DeviceId;
        public Text Txt_FileId;

        public Transform Tf_Login;
        public InputField If_Account;
        public InputField If_Pwd;
        public Button btn_Change;

        public Text Txt_Save_Auto;
        public Button btn_Save;
        public Text Txt_Save;
        public Button btn_Load;
        public Text Txt_Load;

        private const int CdSaveTime = 1800;
        private const int CdLoadTime = 7200;

        // Start is called before the first frame update
        void Start()
        {
            tog_Monster_Skill.onValueChanged.AddListener((isOn) =>
            {
                this.ShowSkill(isOn);
            });
            tog_Monster_Damage.onValueChanged.AddListener((isOn) =>
            {
                this.ShowDamage(isOn);
            });

            if (CheckShow())
            {
                this.btn_Change.onClick.AddListener(this.OnClick_Change);
                this.btn_Save.onClick.AddListener(this.OnClick_Save);
                this.btn_Load.onClick.AddListener(this.OnClick_Load);
            }

            this.Init();
        }

        float currentRoundTime = 0;
        private void Update()
        {
            //if (ConfigHelper.Channel != ConfigHelper.Channel_Tap)
            //{
            this.currentRoundTime += Time.unscaledDeltaTime;
            if (this.currentRoundTime >= 1.0)
            {
                this.currentRoundTime = 0;
                Show();
            }
            //}
        }

        private bool CheckShow()
        {
            if (ConfigHelper.Channel == ConfigHelper.Channel_Tap && !GameProcessor.Inst.Yundang)
            {
                return false;
            }
            return true;
        }

        private void Show()
        {
            if (!CheckShow())
            {
                return;
            }

            User user = GameProcessor.Inst.User;
            string account = user.Account;
            if (account == "")
            {
                return;
            }

            long autoTime = Math.Max(user.SaveTicketTime, user.SaveTickeTimeHand);
            if (autoTime > 0)
            {
                Txt_Save_Auto.text = "最后存档时间:" + TimeHelper.SecondsToDate(autoTime).AddHours(8).ToString("G");
            }
            else
            {
                Txt_Save_Auto.text = "还没有自动存档";
            }

            long now = TimeHelper.ClientNowSeconds();
            long cdSaveTime = now - user.SaveTickeTimeHand;

#if UNITY_EDITOR
            cdSaveTime = 1900;
#endif
            if (cdSaveTime > CdSaveTime)
            {
                btn_Save.gameObject.SetActive(true);
                Txt_Save.gameObject.SetActive(false);
            }
            else
            {
                btn_Save.gameObject.SetActive(false);
                Txt_Save.gameObject.SetActive(true);
                Txt_Save.text = TimeSpan.FromSeconds(CdSaveTime - cdSaveTime).ToString(@"hh\:mm\:ss");
            }

            long cdLoadTime = now - user.LoadTicketTime;
#if UNITY_EDITOR
            cdLoadTime = 8000;
#endif
            if (cdLoadTime > CdLoadTime)
            {
                btn_Load.gameObject.SetActive(true);
                Txt_Load.gameObject.SetActive(false);
            }
            else
            {
                btn_Load.gameObject.SetActive(false);
                Txt_Load.gameObject.SetActive(true);
                Txt_Load.text = TimeSpan.FromSeconds(CdLoadTime - cdLoadTime).ToString(@"hh\:mm\:ss");
            }
        }

        public void Init()
        {
            Debug.Log("Other init");
            tog_Monster_Skill.isOn = GameProcessor.Inst.User.ShowMonsterSkill;
            tog_Monster_Damage.isOn = GameProcessor.Inst.User.ShowMonsterDamage;

            User user = GameProcessor.Inst.User;

            string account = user.Account;
            //this.txt_Account.text = "设备Id:" + id;

            if (!CheckShow())
            {
                this.Tf_Login.gameObject.SetActive(false);
                this.btn_Save.gameObject.SetActive(false);
                this.btn_Load.gameObject.SetActive(false);
                this.txt_Memo.gameObject.SetActive(false);
            }
            else
            {

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
                    this.txt_Memo.text = buildMeme(account);
                }
            }

            this.Txt_Save_Auto.text = "";
            this.Txt_FileId.text = "存档Id:" + user.DeviceId;
            this.Txt_DeviceId.text = "设备Id:" + AppHelper.GetDeviceIdentifier();

            this.Show();

            TaskHelper.CheckTask(TaskType.BindAccount, 1);
        }

        public void ShowSkill(bool show)
        {
            GameProcessor.Inst.User.ShowMonsterSkill = show;
        }
        public void ShowDamage(bool show)
        {
            GameProcessor.Inst.User.ShowMonsterDamage = show;
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
                         this.txt_Memo.text = buildMeme(account);
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

        private string buildMeme(string account)
        {
            string name = ConfigHelper.Channel == ConfigHelper.Channel_Tap ? "TAP" : "QQ";
            return "您已经绑定了帐号,您的存档帐号为:" + account + "\n"
                                + "第一次绑定,请务必先点击保存按钮,以防丢档。\n"
                               + "如果您需要换设备，请在新设备输入帐号和密码，\n"
                               + "再点击绑定，最后点击读取存档。\n"
                               + "一天读档最多次数为5次。\n"
                               + "请不要一个存档绑定太多设备，会导致封号。\n" + name;

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
            user.SaveTickeTimeHand = TimeHelper.ClientNowSeconds();
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
                                this.txt_Info.text = "存档失败." + result.Msg;
                                user.SaveTickeTimeHand = TimeHelper.ClientNowSeconds() - CdSaveTime;
                            }
                        },
                        () =>
                        {
                            btn_Load.gameObject.SetActive(true);
                            this.txt_Info.text = "存档失败.";
                            user.SaveTickeTimeHand = TimeHelper.ClientNowSeconds() - CdSaveTime;
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
            string account = user.Account;

            try
            {

                StartCoroutine(NetworkHelper.DownData(
                  (byte[] bytes) =>
                  {
                      Time.timeScale = 0;

                      if (bytes == null)
                      {
                          this.txt_Info.text = "读档失败,还没有存档或者其他错误.";
                          user.LoadTicketTime = TimeHelper.ClientNowSeconds() - CdLoadTime + 10;
                          return;
                      }

                      string str_json = Encoding.UTF8.GetString(bytes);

                      if (str_json.Length < 100)
                      {
                          WebResultWrapper result = JsonConvert.DeserializeObject<WebResultWrapper>(str_json);
                          this.txt_Info.text = result.Msg;
                          user.LoadTicketTime = TimeHelper.ClientNowSeconds() - CdLoadTime + 10;
                          return;
                      }

                      str_json = EncryptionHelper.AesDecrypt(str_json);

                      if (GameProcessor.Inst.LoadInit(str_json, account))
                      {
                          this.txt_Info.text = "读取存档成功,请退出重进";
                          UserData.Save();
                      }
                      else
                      {
                          this.txt_Info.text = "读取失败,存档损坏,取消读档,请退出重进";
                          user.LoadTicketTime = TimeHelper.ClientNowSeconds() - CdLoadTime + 10;

                      }
                      Application.Quit();
                  },
                  () =>
                  {
                      btn_Load.gameObject.SetActive(true);
                      user.LoadTicketTime = TimeHelper.ClientNowSeconds() - CdLoadTime + 10;
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
