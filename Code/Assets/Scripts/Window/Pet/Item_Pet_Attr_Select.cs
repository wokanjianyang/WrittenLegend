using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Pet_Attr_Select : MonoBehaviour
    {
        public Text Txt_Base;
        public Toggle toggle;

        public delegate void PetAttrSelectEvent(int index);

        private PetAttrSelectEvent selectEvent;
        private int Index = -1;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Base.text = "";
        }

        // Update is called once per frame
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                OnSelect(isOn);
            });
        }

        public void Init(ToggleGroup group)
        {
            this.toggle.group = group;
        }

        public void SetItem(int attrId, long attrValue, int honeLvel, int layer)
        {
            EquipHoneConfig config = EquipHoneConfigCategory.Instance.GetByAttrId(attrId);

            string attrName = StringHelper.FormatAttrValueName(attrId);

            long total = honeLvel * config.AttrValue;

            Txt_Base.text = attrName + StringHelper.FormatAttrValueText(attrId, attrValue) + "+" + StringHelper.FormatAttrValueText(attrId, total);

            int MaxLevel = EquipHoneConfigCategory.Instance.GetMaxLevel(attrId, attrValue, layer);
        }

        public void SetText(string text)
        {
            Txt_Base.text = text;
        }

        public void SetSelect()
        {
            this.toggle.isOn = true;
        }

        public void Clear()
        {
            this.toggle.isOn = false;
        }

        private void OnSelect(bool isOn)
        {
            if (isOn)
            {
                selectEvent?.Invoke(Index);
            }
            else
            {
            }
        }

        public void AddListener(PetAttrSelectEvent e, int index)
        {
            this.Index = index;
            this.selectEvent += e;
        }
    }
}
