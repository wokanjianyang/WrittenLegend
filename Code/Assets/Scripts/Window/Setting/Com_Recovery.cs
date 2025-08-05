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
        [Title("普通装备")]
        public Dropdown dp_Equip_Skill;
        public InputField if_Lucky;
        public InputField if_Gold;
        public InputField if_Exp;
        public InputField if_DropRate;
        public InputField if_DropQuality;

        public Dropdown Dp_Equip_Base;

        public Transform tran_EquipRoleList;
        private Toggle[] equipRoleToggles;

        public InputField if_EquipLevel;

        [Title("红色装备")]
        public Toggle Equip_Red_Open;
        public Toggle Equip_Red_Skill;
        public InputField if_Red_Gold;
        public InputField if_Red_Exp;
        public InputField if_Red_DropRate;
        public InputField if_Red_DropQuality;

        [Title("金色装备")]
        public Toggle Equip_Golden_Open;
        public Toggle Equip_Golden_Skill;
        public InputField if_Golden_Total;

        [Title("暗金装备")]
        public Toggle Equip_Dark_Open;
        public Toggle Equip_Dark_Skill;
        public InputField if_Dark_Total;

        [Title("普通专属")]
        public Dropdown dp_Exclusive_Base_Skill;
        public Dropdown dp_Exclusive_Base;

        [Title("传奇专属")]
        public Dropdown dp_Golden_Skill;
        public Dropdown dp_Exclusive_Golden;

        [Title("不朽专属")]
        public Dropdown dp_Dark_Skill;
        public Dropdown dp_Exclusive_Dark;


        [Title("其他")]
        public InputField ifSpeicalLevel;
        public InputField if_Halidom;
        public InputField if_RedStone;
        public Dropdown dp_Pet;

        public Button btn_Done;

        // Start is called before the first frame update

        private int startQuality = 4;

        public List<Toggle> Panel_Toggles;
        public List<Transform> Panels;


        void Start()
        {
            this.btn_Done.onClick.AddListener(this.OnClick_Done);

            for (int i = 0; i < Panel_Toggles.Count; i++)
            {
                int index = i;

                Panel_Toggles[i].onValueChanged.AddListener((isOn) =>
                {
                    this.ShowPanel(index);
                });

            }

            this.Init();


        }

        private void ShowPanel(int index)
        {
            for (int i = 0; i < Panels.Count; i++)
            {
                this.Panels[i].gameObject.SetActive(i == index);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init()
        {
            Debug.Log("Init Recovery");

            //初始化
            equipRoleToggles = tran_EquipRoleList.GetComponentsInChildren<Toggle>();

            //普通装备保留颜色
            dp_Equip_Skill.ClearOptions();
            dp_Equip_Skill.AddOptions(new List<string>() { "无", "紫色", "橙色" });

            Dp_Equip_Base.ClearOptions();
            Dp_Equip_Base.AddOptions(new List<string>() { "无", "白色", "绿色", "蓝色", "紫色", "橙色" });

            dp_Pet.ClearOptions();
            dp_Pet.AddOptions(new List<string>() { "无", "白色", "绿色", "蓝色", "紫色", "橙色", "红色" });

            //专属
            dp_Exclusive_Base_Skill.ClearOptions();
            dp_Exclusive_Base_Skill.AddOptions(new List<string>() { "无", "紫色", "橙色" });

            dp_Exclusive_Base.ClearOptions();
            dp_Exclusive_Base.AddOptions(new List<string>() { "无", "白色", "绿色", "蓝色", "紫色", "橙色" });

            //传奇专属
            dp_Golden_Skill.ClearOptions();
            dp_Golden_Skill.AddOptions(new List<string>() { "无", "橙色", "红色", "金色" });

            dp_Exclusive_Golden.ClearOptions();
            dp_Exclusive_Golden.AddOptions(new List<string>() { "无", "白色", "绿色", "蓝色", "紫色", "橙色", "红色", "金色" });

            //不朽专属
            dp_Dark_Skill.ClearOptions();
            dp_Dark_Skill.AddOptions(new List<string>() { "无", "橙色", "红色", "金色", "暗金" });

            dp_Exclusive_Dark.ClearOptions();
            dp_Exclusive_Dark.AddOptions(new List<string>() { "无", "白色", "绿色", "蓝色", "紫色", "橙色", "红色", "金色", "暗金" });

            //

            User user = GameProcessor.Inst.User;
            RecoverySettingNew setting = user.RecoveryNew;

            if (setting.EquipQualityKeep > 0)
            {
                dp_Equip_Skill.value = setting.EquipQualityKeep;
            }

            if (setting.EquipQualityRecovery > 0)
            {
                Dp_Equip_Base.value = setting.EquipQualityRecovery;
            }

            if_Exp.text = setting.ExpTotal.ToString();
            if_Gold.text = setting.GoldTotal.ToString();
            if_Lucky.text = setting.LuckyTotal.ToString();
            if_DropRate.text = setting.DropRate.ToString();
            if_DropQuality.text = setting.DropQuality.ToString();

            if_EquipLevel.text = setting.EquipLevel.ToString();

            foreach (int key in setting.EquipRole.Keys)
            {
                equipRoleToggles[key - 1].isOn = setting.EquipRole[key];
            }

            //其他
            ifSpeicalLevel.text = setting.SpecailLevel.ToString();
            if_Halidom.text = setting.HalidomLevel.ToString();
            if_RedStone.text = setting.RedStoneLevel.ToString();
            if (setting.PetQuality > 0)
            {
                dp_Pet.value = setting.PetQuality;
            }

            //红色装备
            Equip_Red_Open.isOn = setting.RedRecovery;
            Equip_Red_Skill.isOn = setting.RedKeep;

            if_Red_Exp.text = setting.RedExpTotal.ToString();
            if_Red_Gold.text = setting.RedGoldTotal.ToString();
            if_Red_DropRate.text = setting.RedDropRate.ToString();
            if_Red_DropQuality.text = setting.RedDropQuality.ToString();

            //金色装备
            Equip_Golden_Open.isOn = setting.EquipiGoldenRecovery;
            Equip_Golden_Skill.isOn = setting.EquipiGoldenKeep;
            if_Golden_Total.text = setting.EquipGoldenTotal.ToString();

            //暗金装备
            Equip_Dark_Open.isOn = setting.EquipiDarkRecovery;
            Equip_Dark_Skill.isOn = setting.EquipiDarkKeep;
            if_Dark_Total.text = setting.EquipDarkTotal.ToString();

            //普通专属
            dp_Exclusive_Base_Skill.value = setting.Exclusive_Recovery;
            dp_Exclusive_Base.value = setting.Exclusive_Keep;

            //传奇专属
            dp_Golden_Skill.value = setting.Exclusive_Recovery_Golden;
            dp_Exclusive_Golden.value = setting.Exclusive_Keep_Golden;

            //不朽专属
            dp_Dark_Skill.value = setting.Exclusive_Recovery_Dark;
            dp_Exclusive_Dark.value = setting.Exclusive_Keep_Dark;

            //for (int i = 0; i < equipQualityToggles.Length; i++)
            //{
            //    if (!setting.EquipQuanlity.ContainsKey(i + 1))
            //    {
            //        setting.EquipQuanlity[i + 1] = false;
            //    }
            //    equipQualityToggles[i].isOn = setting.EquipQuanlity[i + 1];
            //}

            //if (user.Cycle.Data >= 4)
            //{
            //    equipQualityToggles[5].gameObject.SetActive(true);
            //}
            //else
            //{
            //    equipQualityToggles[5].isOn = false;
            //    equipQualityToggles[5].gameObject.SetActive(false);
            //}

            //foreach (int quality in setting.ExclusiveQuanlity.Keys)
            //{
            //    exclusiveToggles[quality - 1].isOn = setting.ExclusiveQuanlity[quality];
            //}


            //for (int i = 0; i < skillToggles.Length; i++)
            //{
            //    skillToggles[i].isOn = setting.GetSkillReserveQuanlity(i + startQuality);//紫色开始
            //}

            //ifSpeicalLevel.text = setting.SpecailLevel.ToString();
            //if_RedStone.text = setting.RedStoneLevel.ToString();
            //if_Halidom.text = setting.HalidomLevel.ToString();
        }


        public void OnClick_Done()
        {
            this.SaveSetting();
        }

        private void SaveSetting()
        {
            User user = GameProcessor.Inst.User;

            RecoverySettingNew setting = GameProcessor.Inst.User.RecoveryNew;

            //普通装备回收
            setting.EquipQualityKeep = dp_Equip_Skill.value;
            setting.EquipQualityRecovery = Dp_Equip_Base.value;

            int.TryParse(if_Exp.text, out int exp);
            setting.ExpTotal = exp;

            int.TryParse(if_Gold.text, out int gold);
            setting.GoldTotal = gold;

            int.TryParse(if_Lucky.text, out int lucky);
            setting.LuckyTotal = lucky;

            int.TryParse(if_DropRate.text, out int dropRate);
            setting.DropRate = dropRate;

            int.TryParse(if_DropQuality.text, out int dropQuality);
            setting.DropQuality = dropQuality;

            int.TryParse(if_EquipLevel.text, out int equipLevel);
            setting.EquipLevel = equipLevel;

            for (var i = 0; i < equipRoleToggles.Length; i++)
            {
                setting.EquipRole[i + 1] = equipRoleToggles[i].isOn;
            }

            //红色装备回收
            setting.RedRecovery = Equip_Red_Open.isOn;
            setting.RedKeep = Equip_Red_Skill.isOn;

            int.TryParse(if_Red_Exp.text, out int redexp);
            setting.RedExpTotal = redexp;

            int.TryParse(if_Red_Gold.text, out int redgold);
            setting.RedGoldTotal = redgold;

            int.TryParse(if_Red_DropRate.text, out int reddropRate);
            setting.RedDropRate = reddropRate;

            int.TryParse(if_Red_DropQuality.text, out int reddropQuality);
            setting.RedDropQuality = reddropQuality;

            //金色装备
            setting.EquipiGoldenRecovery = Equip_Golden_Open.isOn;
            setting.EquipiGoldenKeep = Equip_Golden_Skill.isOn;
            int.TryParse(if_Golden_Total.text, out int goldenexp);
            setting.EquipGoldenTotal = goldenexp;

            //暗金装备
            setting.EquipiDarkRecovery = Equip_Dark_Open.isOn;
            setting.EquipiDarkKeep = Equip_Dark_Skill.isOn;
            int.TryParse(if_Dark_Total.text, out int darkexp);
            setting.EquipDarkTotal = darkexp;

            //其他回收
            setting.PetQuality = dp_Pet.value;

            int.TryParse(ifSpeicalLevel.text, out int speicalLevel);
            setting.SpecailLevel = speicalLevel;

            int.TryParse(if_Halidom.text, out int halidomLevel);
            setting.HalidomLevel = halidomLevel;

            int.TryParse(if_RedStone.text, out int redStoneLevel);
            setting.RedStoneLevel = redStoneLevel;


            //普通专属
            setting.Exclusive_Recovery = dp_Exclusive_Base_Skill.value;
            setting.Exclusive_Keep = dp_Exclusive_Base.value;

            //传奇专属
            setting.Exclusive_Recovery_Golden = dp_Golden_Skill.value;
            setting.Exclusive_Keep_Golden = dp_Exclusive_Golden.value;

            //不朽专属
            setting.Exclusive_Recovery_Dark = dp_Dark_Skill.value;
            setting.Exclusive_Keep_Dark = dp_Exclusive_Dark.value;




            //立即执行一次回收
            //GameProcessor.Inst.EventCenter.Raise(new AutoRecoveryEvent() { RuleType = RuleType.Normal });

            //TaskHelper.CheckTask(TaskType.Recovery, 1);

            //GameProcessor.Inst.SaveData();

            GameProcessor.Inst.EventCenter.Raise(new DialogSettingEvent());

        }
    }
}
