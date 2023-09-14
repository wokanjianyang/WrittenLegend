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

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Done.onClick.AddListener(this.OnClick_Done);
            this.btn_Cancle.onClick.AddListener(this.OnClick_Cancle);
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
    }
}
