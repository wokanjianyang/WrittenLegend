﻿using SA.Android.Utilities;
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
using TapTap.Bootstrap;
using UnityEngine.Networking;

using TapTap.Common;

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

        public Text txt_Account;
        public Button btn_Change;


        // Start is called before the first frame update
        void Start()
        {
            tog_Monster_Skill.onValueChanged.AddListener((isOn) =>
            {
                this.ShowSkill(isOn);
            });

            this.btn_Change.onClick.AddListener(this.OnClick_Change);

            this.btn_Query.onClick.AddListener(this.OnClick_Query);
            this.btn_Save.onClick.AddListener(this.OnClick_Save);
            this.btn_Load.onClick.AddListener(this.OnClick_Load);

            this.CheckProgress();

            AsyncLoginTap();

        }

        public void ShowSkill(bool show)
        {
            GameProcessor.Inst.User.ShowMonsterSkill = show;
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

        private async Task AsyncLoginTap()
        {
            var currentUser = await TDSUser.GetCurrent();
            if (null == currentUser)
            {
                Debug.Log("开始登录");
                AsyncLoginTap1();
            }
            else
            {
                var nickname = currentUser["nickname"];  // 昵称
                this.txt_Account.text = "Tap帐号:" + nickname;
                UserData.tapAccount = currentUser.ObjectId;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init()
        {
            tog_Monster_Skill.isOn = GameProcessor.Inst.User.ShowMonsterSkill;
        }

        public void OnClick_Change()
        {
            AsyncLoginTap1();
        }

        private async Task AsyncLoginTap1()
        {
            // 开始登录
            try
            {

                // 在 iOS、Android 系统下会唤起 TapTap 客户端或以 WebView 方式进行登录
                // 在 Windows、macOS 系统下显示二维码（默认）和跳转链接（需配置）
                var tdsUser = await TDSUser.LoginWithTapTap();
                //Debug.Log($"login success:{tdsUser}");
                // 获取 TDSUser 属性
                var objectId = tdsUser.ObjectId;     // 用户唯一标识
                var nickname = tdsUser["nickname"];  // 昵称
                var avatar = tdsUser["avatar"];      // 头像

                this.txt_Account.text = "Tap帐号:" + nickname;
                UserData.tapAccount = objectId;
            }
            catch (Exception e)
            {
                if (e is TapException tapError)  // using TapTap.Common
                {
                    //Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
                    if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
                    {
                        //Debug.Log("登录取消");
                    }
                }
            }    // 头像
        }

        public void OnClick_Query()
        {
            this.queryData();
        }

        async Task queryData()
        {
        }

        public void OnClick_Save()
        {
            saveData();
        }

        async Task saveData()
        {
            User user = GameProcessor.Inst.User;
            btn_Save.gameObject.SetActive(false);

            if (user.SaveLimit <= 0)
            {
                this.txt_Name.text = "今天存档次数已满";
                return;
            }

            this.txt_Name.text = "存档中......";

            //先删除旧存档
            var collection = await TapGameSave.GetCurrentUserGameSaves();

            foreach (var oldGameSave in collection)
            {
                await oldGameSave.Delete();
            }

            //再存储新档
            user.DataProgeress++;
            user.SaveLimit--;
            UserData.Save(true);

            string filePath = UserData.getTempPath();
            DateTime time = DateTime.Now.ToLocalTime();

            var gameSave = new TapGameSave
            {
                Name = "UserData",
                Summary = user.Name,
                ModifiedAt = time,
                PlayedTime = 60000L, // ms
                ProgressValue = user.DataProgeress,
                //CoverFilePath = "", // jpg/png
                GameFilePath = filePath,
            };

            var res = await gameSave.Save();

            this.txt_Name.text = res.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

            this.CheckProgress();
        }

        public void OnClick_Load()
        {
            GameProcessor.Inst.SetGameOver(PlayerType.Hero);
            this.loadData();
        }

        private async Task loadData()
        {
            User user = GameProcessor.Inst.User;
            btn_Load.gameObject.SetActive(false);

            if (user.LoadLimit <= 0)
            {
                this.txt_Name.text = "今天读档次数已满";
                return;
            }

            this.txt_Name.text = "读取存档中......";

            var collection = await TapGameSave.GetCurrentUserGameSaves();

            if (collection.Count <= 0)
            {
                this.txt_Name.text = "还没有云存档";
                return;
            }

            string url = "";
            string at = "";

            foreach (var gameFile in collection)
            {
                url = gameFile.GameFile.Url;
                at = gameFile.CreatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            }

            StartCoroutine(DownData(url, at));
        }


        private IEnumerator DownData(string url, string at)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.timeout = 15;

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                this.txt_Name.text = "读取存档失败";
            }
            else
            {
                int tempLoadLimit = GameProcessor.Inst.User.LoadLimit;

                string str_json = Encoding.UTF8.GetString(www.downloadHandler.data);

                //Debug.Log("str_json:" + str_json);

                GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是确认读取" + at + "\n的存档,读档成功之后会自动退出，需要手动进入游戏？", true, () =>
                {
                    this.txt_Name.text = "读取存档成功,请退出重进";
                    UserData.SaveData(str_json, false);

                    GameProcessor.Inst.Init(UserData.StartTime);

                    GameProcessor.Inst.User.DataDate = DateTime.Now.Ticks;
                    GameProcessor.Inst.User.LoadLimit = Math.Min(tempLoadLimit, GameProcessor.Inst.User.LoadLimit) - 1;
                    UserData.Save();
                    this.CheckProgress();

                    //Debug.Log("loadData Success");

                    Application.Quit();
                }, () =>
                {

                });
            }
        }
    }
}
