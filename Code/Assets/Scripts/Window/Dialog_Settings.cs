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
    public class Dialog_Settings : MonoBehaviour, IBattleLife
    {
        public Com_Recovery com_Recovery;
        public Com_Other com_Other;
        public Com_Settings com_Settings;

        public Toggle tog_Recovery;
        public Toggle tog_Base;
        public Toggle tog_Other;

        public Button Btn_Close;

        public int Order => (int)ComponentOrder.Dialog;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Close.onClick.AddListener(this.OnClick_Close);
        }

        public void OnBattleStart()
        {
            this.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.AddListener<DialogSettingEvent>(this.OnEquipRecoveryEvent);
        }

        public void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }

        private void OnEquipRecoveryEvent(DialogSettingEvent e)
        {
            this.gameObject.SetActive(e.IsOpen);
            if (e.IsOpen)
            {
                this.tog_Recovery.isOn = true;
                com_Recovery.Open();

                com_Other.Init();
            }
        }
    }
}
