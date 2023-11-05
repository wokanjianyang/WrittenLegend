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
        [LabelText("装备品质设置")]
        public Transform tran_EquipQualityList;
        [LabelText("装备职业设置")]
        public Transform tran_EquipRoleList;
        [LabelText("装备等级")]
        public InputField if_EquipLevel;
        [LabelText("四格等级")]
        public InputField ifSpeicalLevel;

        [LabelText("随机幸运属性")]
        public InputField if_Lucky;
        [LabelText("随机金币属性")]
        public InputField if_Gold;
        [LabelText("随机经验属性")]
        public InputField if_Exp;
        [LabelText("随机爆率属性")]
        public InputField if_DropRate;
        [LabelText("随机品质属性")]
        public InputField if_DropQuality;

        [LabelText("确认")]
        public Button btn_Done;
        
        [LabelText("取消")]
        public Button btn_Cancle;

        Toggle[] equipQualityToggles;
        Toggle[] equipRoleToggles;

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
            equipQualityToggles = tran_EquipQualityList.GetComponentsInChildren<Toggle>();
            equipRoleToggles = tran_EquipRoleList.GetComponentsInChildren<Toggle>();
                
            User user = GameProcessor.Inst.User;
            RecoverySetting setting = user.RecoverySetting;

            foreach (int equipQuality in setting.EquipQuanlity.Keys)
            {
                if (setting.EquipQuanlity[equipQuality])
                {
                    equipQualityToggles[equipQuality - 1].isOn = true;
                }
                else
                {
                    equipQualityToggles[equipQuality - 1].isOn = false;
                }
            }

            if_EquipLevel.text = setting.EquipLevel.ToString();
            if_Exp.text = setting.ExpTotal.ToString();
            if_Gold.text = setting.GoldTotal.ToString();
            if_Lucky.text = setting.LuckyTotal.ToString();
            if_DropRate.text = setting.DropRate.ToString();
            if_DropQuality.text = setting.DropQuality.ToString();
            ifSpeicalLevel.text= setting.SpecailLevel.ToString();

            foreach (int skillBookRole in setting.EquipRole.Keys)
            {
                if (setting.EquipRole[skillBookRole])
                {
                    equipRoleToggles[skillBookRole - 1].isOn = true;
                }
                else
                {
                    equipRoleToggles[skillBookRole - 1].isOn = false;
                }
            }

        }

        public void OnClick_Done()
        {
            this.SaveSetting();
        }
        
        public void OnClick_Cancle()
        {
            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());
        }

        private void SaveSetting()
        {
            User user = GameProcessor.Inst.User;

            for (var i = 0; i < equipQualityToggles.Length; i++)
            {
                var toggle = equipQualityToggles[i];
                if (toggle.isOn)
                {
                    user.RecoverySetting.SetEquipQuanlity(i + 1, true);
                }
                else
                {
                    user.RecoverySetting.SetEquipQuanlity(i + 1, false);
                }
            }

            int.TryParse(if_EquipLevel.text, out int equipLevel);
            user.RecoverySetting.EquipLevel = equipLevel;

            int.TryParse(ifSpeicalLevel.text, out int speicalLevel);
            user.RecoverySetting.SpecailLevel = speicalLevel;

            for (var i = 0; i < equipRoleToggles.Length; i++)
            {
                var toggle = equipRoleToggles[i];
                if (toggle.isOn)
                {
                    user.RecoverySetting.SetEquipRole(i + 1, true);
                }
                else
                {
                    user.RecoverySetting.SetEquipRole(i + 1, false);
                }
            }

            int.TryParse(if_Exp.text, out int exp);
            user.RecoverySetting.ExpTotal = exp;

            int.TryParse(if_Gold.text, out int gold);
            user.RecoverySetting.GoldTotal = gold;

            int.TryParse(if_Lucky.text, out int lucky);
            user.RecoverySetting.LuckyTotal = lucky;

            int.TryParse(if_DropRate.text, out int dropRate);
            user.RecoverySetting.DropRate = dropRate;

            int.TryParse(if_DropQuality.text, out int dropQuality);
            user.RecoverySetting.DropQuality = dropQuality;

            //立即执行一次回收
            GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { });

            TaskHelper.CheckTask(TaskType.Recovery, 1);

            UserData.Save();

            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());

        }
    }
}
