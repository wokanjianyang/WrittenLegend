using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Exclusive : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;
        public Toggle toggle;

        public ExclusiveItem item;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Name.text = "";
            Txt_Level.text = "";
            Txt_Layer.text = "";
        }

        // Update is called once per frame
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                Select(isOn);
            });
        }

        public void Init(ExclusiveItem equip, ToggleGroup group)
        {
            this.item = equip;

            Txt_Name.text = equip.ExclusiveConfig.Name;
            Txt_Level.text = ConfigHelper.LayerChinaList[equip.GetLevel()] + "��";

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
                GameProcessor.Inst.EventCenter.Raise(new ExclusiveUpEvent()
                {
                    Exclusive = this.item,
                });
            }
        }
    }
}
