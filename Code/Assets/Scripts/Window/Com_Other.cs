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

namespace Game
{
    public class Com_Other : MonoBehaviour
    {
        [LabelText("显示怪物技能特效")]
        public Toggle tog_Monster_Skill;

        [LabelText("确认")]
        public Button btn_Done;

        [LabelText("取消")]
        public Button btn_Cancle;

        public Button btn_Save;
        public Button btn_Load;
        public Text txt_Name;

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Done.onClick.AddListener(this.OnClick_Done);
            this.btn_Cancle.onClick.AddListener(this.OnClick_Cancle);
            this.btn_Save.onClick.AddListener(this.OnClick_Save);
            this.btn_Load.onClick.AddListener(this.OnClick_Load);

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init()
        {
            tog_Monster_Skill.isOn = GameProcessor.Inst.User.ShowMonsterSkill;
        }

        public void OnClick_Cancle()
        {
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());
        }

        public void OnClick_Done()
        {
            User user = GameProcessor.Inst.User;

            user.ShowMonsterSkill = tog_Monster_Skill.isOn;

            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());
        }
        public void OnClick_Save()
        {
            //AN_Preloader.LockScreen("保存存档...");
            StartCoroutine(this.saveData());
        }

        private IEnumerator saveData()
        {
            yield return UserData.SaveTapData();

            yield return null;
            //txt_Name.text = "存档时间:" + time;
        }


        public void OnClick_Load()
        {
            StartCoroutine(this.loadData());
        }

        private IEnumerator loadData()
        {
            yield return UserData.DownData();
        }

    }
}
