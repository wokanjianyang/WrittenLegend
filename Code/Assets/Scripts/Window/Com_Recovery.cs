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
    public class Com_Recovery : MonoBehaviour
    {
        [LabelText("装备回收设置")]
        public Transform tran_EquipQualityList;

        // [LabelText("装备等级")]
        // public Toggle toggle_EquipLevel;

        [LabelText("装备等级")]
        public InputField if_EquipLevel;
        
        [LabelText("秘籍回收设置")]
        public Transform tran_BookJobList;

        [LabelText("秘籍等级")]
        public Toggle toggle_BookLevel;
        
        [LabelText("装备等级")]
        public Dropdown dd_BookLevel;
        
        [LabelText("确认")]
        public Button btn_Done;
        
        [LabelText("取消")]
        public Button btn_Cancle;

        Toggle[] equipToggles;
        Toggle[] bookToggles;
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

        public void Open()
        {
            //初始化
            equipToggles = tran_EquipQualityList.GetComponentsInChildren<Toggle>();
            bookToggles = tran_BookJobList.GetComponentsInChildren<Toggle>();
                
            User user = GameProcessor.Inst.User;
            RecoverySetting setting = user.RecoverySetting;

            foreach (int equipQuality in setting.EquipQuanlity.Keys)
            {
                if (setting.EquipQuanlity[equipQuality])
                {
                    equipToggles[equipQuality - 1].isOn = true;
                }
                else
                {
                    equipToggles[equipQuality - 1].isOn = false;
                }
            }

            if_EquipLevel.text = setting.EquipLevel.ToString();


            foreach (int skillBookRole in setting.EquipRole.Keys)
            {
                if (setting.EquipRole[skillBookRole])
                {
                    bookToggles[skillBookRole - 1].isOn = true;
                }
                else
                {
                    bookToggles[skillBookRole - 1].isOn = false;
                }
            }

            if (setting.SkillBookLevel > 0)
            {
                toggle_BookLevel.isOn = true;
                dd_BookLevel.value = Math.Max(1, setting.SkillBookLevel / 10) - 1;
            }
            else {
                toggle_BookLevel.isOn = false;
            }
        }

        public void OnClick_Done()
        {
            if (equipToggles[3].isOn)
            {
                GameProcessor.Inst.ShowSecondaryConfirmationDialog?.Invoke("是否确认回收紫色品质？",true, () =>
                {
                    this.SaveSetting();
                }, null);
            }
            else
            {
                this.SaveSetting();
            }
        }
        
        public void OnClick_Cancle()
        {
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());
        }
        
        private void SaveSetting()
        {
            User user = GameProcessor.Inst.User;

            StringBuilder sb = new StringBuilder();

            sb.Append("装备自动回收 品质：");
            for (var i = 0; i < equipToggles.Length; i++)
            {
                var toggle = equipToggles[i];
                if (toggle.isOn)
                {
                    user.RecoverySetting.SetEquipQuanlity(i + 1, true);
                }
                else
                {
                    user.RecoverySetting.SetEquipQuanlity(i + 1, false);
                }
            }

            // if (toggle_EquipLevel.isOn)
            {
                int level = 0;
                int.TryParse(if_EquipLevel.text, out level);

                user.RecoverySetting.EquipLevel = level;
            }
            // else {
                // user.RecoverySetting.EquipLevel = 0;
            // }

            sb.Append("装备自动回收 职业：");
            for (var i = 0; i < bookToggles.Length; i++)
            {
                var toggle = bookToggles[i];
                if (toggle.isOn)
                {
                    user.RecoverySetting.SetEquipRole(i + 1, true);
                }
                else
                {
                    user.RecoverySetting.SetEquipRole(i + 1, false);
                }
            }
            if (toggle_BookLevel.isOn)
            {
                sb.Append("等级：");
                sb.Append(dd_BookLevel.options[dd_BookLevel.value].text);

                int bookLevel = 0;
                int.TryParse(dd_BookLevel.options[dd_BookLevel.value].text, out bookLevel);

                user.RecoverySetting.SkillBookLevel = bookLevel;
            }
            else {
                user.RecoverySetting.SkillBookLevel = 0;
            }

            Log.Debug($"回收设置 {sb.ToString()}");
                
            //立即执行一次回收
            GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });

            TaskHelper.CheckTask(TaskType.Recovery, 1);

            UserData.Save();
            
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());
            
        }
    }
}
