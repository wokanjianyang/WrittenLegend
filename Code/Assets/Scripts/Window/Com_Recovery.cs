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

        public Transform tran_ExclusiveList;

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

        Toggle[] equipQualityToggles;
        Toggle[] equipRoleToggles;
        Toggle[] exclusiveToggles;
        public Toggle[] skillToggles;
        // Start is called before the first frame update

        private int startQuality = 4;

        void Start()
        {
            this.btn_Done.onClick.AddListener(this.OnClick_Done);

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
            exclusiveToggles = tran_ExclusiveList.GetComponentsInChildren<Toggle>();

            User user = GameProcessor.Inst.User;
            RecoverySetting setting = user.RecoverySetting;

            foreach (int equipQuality in setting.EquipQuanlity.Keys)
            {
                equipQualityToggles[equipQuality - 1].isOn = setting.EquipQuanlity[equipQuality];
            }

            foreach (int quality in setting.ExclusiveQuanlity.Keys)
            {
                exclusiveToggles[quality - 1].isOn = setting.ExclusiveQuanlity[quality];
            }

            foreach (int skillBookRole in setting.EquipRole.Keys)
            {
                equipRoleToggles[skillBookRole - 1].isOn = setting.EquipRole[skillBookRole];
            }

            for (int i = 0; i < skillToggles.Length; i++)
            {
                skillToggles[i].isOn = setting.GetSkillReserveQuanlity(i + startQuality);//紫色开始
            }


            if_EquipLevel.text = setting.EquipLevel.ToString();
            if_Exp.text = setting.ExpTotal.ToString();
            if_Gold.text = setting.GoldTotal.ToString();
            if_Lucky.text = setting.LuckyTotal.ToString();
            if_DropRate.text = setting.DropRate.ToString();
            if_DropQuality.text = setting.DropQuality.ToString();
            ifSpeicalLevel.text = setting.SpecailLevel.ToString();
        }

        public void OnClick_Done()
        {
            this.SaveSetting();
        }

        private void SaveSetting()
        {
            User user = GameProcessor.Inst.User;

            for (var i = 0; i < equipQualityToggles.Length; i++)
            {
                user.RecoverySetting.SetEquipQuanlity(i + 1, equipQualityToggles[i].isOn);
            }

            for (var i = 0; i < exclusiveToggles.Length; i++)
            {
                user.RecoverySetting.SetExclusiveQuanlity(i + 1, exclusiveToggles[i].isOn);
            }

            int.TryParse(if_EquipLevel.text, out int equipLevel);
            user.RecoverySetting.EquipLevel = equipLevel;

            int.TryParse(ifSpeicalLevel.text, out int speicalLevel);
            user.RecoverySetting.SpecailLevel = speicalLevel;

            for (var i = 0; i < equipRoleToggles.Length; i++)
            {
                user.RecoverySetting.SetEquipRole(i + 1, equipRoleToggles[i].isOn);
            }

            for (int i = 0; i < skillToggles.Length; i++)
            {
                user.RecoverySetting.SkillReserveQuanlity[i + startQuality] = skillToggles[i].isOn;
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
            GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { RuleType = RuleType.Normal });

            TaskHelper.CheckTask(TaskType.Recovery, 1);

            UserData.Save();

            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());

        }
    }
}
