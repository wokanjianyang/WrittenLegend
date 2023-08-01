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
        
        // Start is called before the first frame update
        void Start()
        {
            this.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        public int Order => (int)ComponentOrder.Dialog;

        public void OnBattleStart()
        {
            GameProcessor.Inst.EventCenter.AddListener<DialogSettingEvent>(this.OnEquipRecoveryEvent);

        }

        private void OnEquipRecoveryEvent(DialogSettingEvent e)
        {
            this.gameObject.SetActive(e.IsOpen);

        }
    }
}
