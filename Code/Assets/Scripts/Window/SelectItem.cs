using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class SelectItem : MonoBehaviour
    {
        public Toggle Tg_Bg;
        public Text Txt_Name;

        public BoxItem BoxItem { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            this.Tg_Bg.onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    ShowDetail();
                }
            });
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItem(BoxItem box, ToggleGroup group)
        {
            this.BoxItem = box;

            this.Txt_Name.text = box.Item.Name;

            this.Tg_Bg.group = group;
        }

        private void ShowDetail()
        {
            if (this.BoxItem == null) return;

            if (this.BoxItem.Item.Type == ItemType.Exclusive)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowExclusiveCardEvent()
                {
                    boxItem = this.BoxItem,
                    EquipPosition = -2,
                    Type = ComBoxType.Bag,
                });
                return;
            }
            else
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowEquipDetailEvent()
                {
                    boxItem = this.BoxItem,
                    EquipPosition = -2
                });
            }
        }
    }
}
