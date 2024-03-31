using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class ItemRefresh : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public Equip item;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Name.text = "";
            Txt_Level.text = "";
        }

        // Update is called once per frame
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                Select(isOn);
            });
        }

        public void Init(Equip equip, ToggleGroup group)
        {
            this.item = equip;

            Txt_Name.text = equip.EquipConfig.Name;
            Txt_Level.text = ConfigHelper.LayerChinaList[equip.Layer] + "½×";

            this.toggle.group = group;
        }

        public void Clear()
        {
            this.toggle.isOn = false;
            this.item = null;
        }

        private void Select(bool isOn)
        {
            if (isOn && item != null)
            {
                GameProcessor.Inst.EventCenter.Raise(new RefershSelectEvent()
                {
                    Equip = this.item,
                });
            }
        }
    }
}
