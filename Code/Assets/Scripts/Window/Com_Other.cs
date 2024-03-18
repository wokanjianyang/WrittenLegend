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

            string id = GameProcessor.Inst.User.DeviceId;
            //this.txt_Account.text = "设备Id:" + id;

            this.Txt_Account.text = "存档Id:" + GameProcessor.Inst.User.DeviceId;
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
            //AsyncLoginTap1();
        }

        public void OnClick_Load()
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Hero);
            //this.loadData();
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


            //再存储新档
            StartCoroutine(Upload(
                () => { this.txt_Name.text = "存档成功."; },
                () => { this.txt_Name.text = "存档失败."; }
                ));


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

        private IEnumerator Upload(Action successAction, Action failAction)
        {
            string url = "http://127.0.0.1:11111/public/save";

            // 读取文件
            string filePath = UserData.getSavePath();
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            Debug.Log("fileBytes:" + fileBytes.Length);

            WWWForm form = new WWWForm();
            form.AddField("data", "{'id':123}");
            form.AddBinaryData("file", System.IO.File.ReadAllBytes(filePath), "file.bin", "application/octet-stream");

            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                // 设置uploadHandler
                www.timeout = 10;
                www.uploadHandler = new UploadHandlerRaw(form.data);
                //www.SetRequestHeader("Content-Type", "multipart/form-data; boundary=" + form.boundary);

                www.disposeDownloadHandlerOnDispose = true;
                www.disposeUploadHandlerOnDispose = true;

                // 发送请求并等待
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Upload Error:" + www.error);

                    //this.txt_Name.text = "存档失败!!!";
                    failAction?.Invoke();
                }
                else
                {
                    Debug.Log("Upload complete! Server response: " + www.downloadHandler.text);

                    successAction?.Invoke();
                    //this.txt_Name.text = "存档成功。";
                }


                www.Dispose();
            }
        }

        //private async Task AsyncLoginTap()
        //{
        //    var currentUser = await TDSUser.GetCurrent();
        //    if (null == currentUser)
        //    {
        //        Debug.Log("开始登录");
        //        AsyncLoginTap1();
        //    }
        //    else
        //    {
        //        var nickname = currentUser["nickname"];  // 昵称
        //        this.txt_Account.text = "Tap帐号:" + nickname;
        //        UserData.tapAccount = currentUser.ObjectId;
        //    }
        //}

        // Update is called once per frame




        //private async Task AsyncLoginTap1()
        //{
        //    // 开始登录
        //    try
        //    {

        //        // 在 iOS、Android 系统下会唤起 TapTap 客户端或以 WebView 方式进行登录
        //        // 在 Windows、macOS 系统下显示二维码（默认）和跳转链接（需配置）
        //        var tdsUser = await TDSUser.LoginWithTapTap();
        //        //Debug.Log($"login success:{tdsUser}");
        //        // 获取 TDSUser 属性
        //        var objectId = tdsUser.ObjectId;     // 用户唯一标识
        //        var nickname = tdsUser["nickname"];  // 昵称
        //        var avatar = tdsUser["avatar"];      // 头像

        //        this.txt_Account.text = "Tap帐号:" + nickname;
        //        UserData.tapAccount = objectId;
        //    }
        //    catch (Exception e)
        //    {
        //        if (e is TapException tapError)  // using TapTap.Common
        //        {
        //            //Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
        //            if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
        //            {
        //                //Debug.Log("登录取消");
        //            }
        //        }
        //    }    // 头像
        //}

        //public void OnClick_Query()
        //{
        //    this.queryData();
        //}

        //async Task queryData()
        //{
        //}
        //private async Task loadData()
        //{
        //    User user = GameProcessor.Inst.User;
        //    btn_Load.gameObject.SetActive(false);

        //    if (user.LoadLimit <= 0)
        //    {
        //        this.txt_Name.text = "今天读档次数已满";
        //        return;
        //    }

        //    this.txt_Name.text = "读取存档中......";

        //    var collection = await TapGameSave.GetCurrentUserGameSaves();

        //    if (collection.Count <= 0)
        //    {
        //        this.txt_Name.text = "还没有云存档";
        //        return;
        //    }

        //    string url = "";
        //    string at = "";

        //    foreach (var gameFile in collection)
        //    {
        //        url = gameFile.GameFile.Url;
        //        at = gameFile.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
        //    }

        //    StartCoroutine(DownData(url, at));
        //}


        //private IEnumerator DownData(string url, string at)
        //{
        //    UnityWebRequest www = UnityWebRequest.Get(url);
        //    www.timeout = 15;

        //    yield return www.SendWebRequest();

        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        this.txt_Name.text = "读取存档失败";
        //    }
        //    else
        //    {
        //        int tempLoadLimit = GameProcessor.Inst.User.LoadLimit;

        //        string str_json = Encoding.UTF8.GetString(www.downloadHandler.data);

        //        //Debug.Log("str_json:" + str_json);

        //        GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是确认读取" + at + "\n的存档,读档成功之后会自动退出，需要手动进入游戏？", true, () =>
        //        {
        //            this.txt_Name.text = "读取存档成功,请退出重进";
        //            UserData.SaveData(str_json, false);

        //            GameProcessor.Inst.Init(UserData.StartTime);

        //            GameProcessor.Inst.User.DataDate = DateTime.Now.Ticks;
        //            GameProcessor.Inst.User.LoadLimit = Math.Min(tempLoadLimit, GameProcessor.Inst.User.LoadLimit) - 1;
        //            UserData.Save();
        //            this.CheckProgress();

        //            //Debug.Log("loadData Success");

        //            Application.Quit();
        //        }, () =>
        //        {

        //        });
        //    }
        //}
    }
}
