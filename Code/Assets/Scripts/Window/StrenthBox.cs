using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class StrenthBox : MonoBehaviour, IPointerClickHandler
    {
        [Title("插槽")]
        [LabelText("类型")]
        public SlotType SlotType;

        public Text Txt_Name;
        public Text Txt_Level;

        public bool Active = false;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Name.text = this.SlotType.ToString();
            Txt_Level.text = "";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetLevel(long level)
        {
            if (level > 0)
            {
                if (level >= 1000000)
                {
                    Txt_Level.text = StringHelper.FormatNumber(level);
                }
                else
                {
                    Txt_Level.text = level + "";
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //高亮，更换UI
            int position = ((int)SlotType);
            GameProcessor.Inst.EventCenter.Raise(new EquipStrengthSelectEvent() { Position = position });

            SeSelect(true);
        }

        public void SeSelect(bool select)
        {
            //Debug.Log("SeSelect :" + (int)SlotType + " " + select);

            this.Active = select;

            if (Active)
            {
                Txt_Name.color = new Color(0xD8 / 255.0f, 0xCA / 255.0f, 0xAF / 255.0f);
            }
            else
            {
                Txt_Name.color = new Color(0xAA / 255.0f, 0xAA / 255.0f, 0xAA / 255.0f, 0x42 / 255.0f);
            }
        }
    }
}
