using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Hone : MonoBehaviour
    {
        public Text Txt_Base;
        public Toggle toggle;
        public Text Txt_Rise;

        public delegate void HoneSelectEvent(int index);

        private HoneSelectEvent selectEvent;
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

            if (honeLvel >= MaxLevel)
            {
                Txt_Rise.text = "已满,请提升装备等阶";
            }
            else
            {
                Txt_Rise.text = "+ " + config.AttrValue + "%";
            }

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

                Txt_Rise.gameObject.SetActive(true);
            }
            else
            {
                Txt_Rise.gameObject.SetActive(false);
            }
        }

        public void AddListener(HoneSelectEvent e, int index)
        {
            this.Index = index;
            this.selectEvent += e;
        }
    }
}
