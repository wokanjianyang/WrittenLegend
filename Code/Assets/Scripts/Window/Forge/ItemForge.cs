using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class ItemForge : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        private int Type = 1;
        private int Position = -1;

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

        public void Init(int type, int position, long level, ToggleGroup group)
        {
            this.Type = type;
            this.Position = position;
            Txt_Name.text = ((SlotType)position).ToString();
            Txt_Level.text = level + "";

            this.toggle.group = group;
        }

        public void SetLevel(long level)
        {
            Txt_Level.text = level + "";
        }


        private void Select(bool isOn)
        {
            if (isOn)
            {
                if (Type == 1)
                {
                    GameProcessor.Inst.EventCenter.Raise(new EquipStrengthSelectEvent() { Position = this.Position });
                }
                else if (Type == 2)
                {
                    GameProcessor.Inst.EventCenter.Raise(new EquipRefineSelectEvent() { Position = this.Position });
                }
            }
        }
    }
}
