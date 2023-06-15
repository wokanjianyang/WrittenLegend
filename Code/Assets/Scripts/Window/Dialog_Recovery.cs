using SA.Android.Utilities;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Dialog_Recovery : MonoBehaviour, IBattleLife
    {
        [LabelText("装备回收设置")]
        public Transform tran_EquipQualityList;

        [LabelText("装备等级")]
        public Toggle toggle_EquipLevel;

        [LabelText("装备等级")]
        public Dropdown dd_EquipLevel;
        
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
        
        // Start is called before the first frame update
        void Start()
        {
            this.btn_Done.onClick.AddListener(this.OnClick_Done);
            this.btn_Cancle.onClick.AddListener(this.OnClick_Cancle);
            this.gameObject.SetActive(false); 
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<EquipRecoveryEvent>(this.OnEquipRecoveryEvent);
        }

        private void OnEquipRecoveryEvent(EquipRecoveryEvent e)
        {
            this.gameObject.SetActive(true);

        }

        public void OnClick_Done()
        {
            this.gameObject.SetActive(false);

            User user = GameProcessor.Inst.User;


            StringBuilder sb = new StringBuilder();
            var equipToggles = tran_EquipQualityList.GetComponentsInChildren<Toggle>();
            sb.Append("装备自动回收 品质：");
            for (var i = 0; i < equipToggles.Length; i++)
            {
                var toggle = equipToggles[i];
                if (toggle.isOn)
                {
                    switch (i)
                    {
                        case 0:
                            user.RecoverySetting.SetQuanlity(1, true);
                            sb.Append("白色,");
                            break;
                        case 1:
                            sb.Append("绿色,");
                            break;
                        case 2:
                            sb.Append("蓝色,");
                            break;
                        case 3:
                            sb.Append("紫色,");
                            break;
                    }
                }
            }

            if (toggle_EquipLevel.isOn)
            {
                string level = dd_EquipLevel.options[dd_EquipLevel.value].text;
                sb.Append("等级：");
                sb.Append(level);
                user.RecoverySetting.SetLevel(int.Parse(level));
            }
            
            sb.Append("秘籍自动回收 职业：");
            var bookToggles = tran_BookJobList.GetComponentsInChildren<Toggle>();
            for (var i = 0; i < bookToggles.Length; i++)
            {
                var toggle = bookToggles[i];
                if (toggle.isOn)
                {
                    switch (i)
                    {
                        case 0:
                            sb.Append("战士,");
                            break;
                        case 1:
                            sb.Append("法师,");
                            break;
                        case 2:
                            sb.Append("道士,");
                            break;
                        case 3:
                            sb.Append("通用,");
                            break;
                    }
                }
            }

            if (toggle_BookLevel.isOn)
            {
                sb.Append("等级：");
                sb.Append(dd_BookLevel.options[dd_BookLevel.value].text);
            }
            
            Log.Debug($"回收设置 {sb.ToString()}");

            UserData.Save();
        }
        
        public void OnClick_Cancle()
        {
            this.gameObject.SetActive(false); 
        }
    }
}
