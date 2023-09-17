using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class ExclusiveBox : MonoBehaviour
    {
        [Title("插槽")]
        [LabelText("类型")]
        public SlotType SlotType;

        public Text Txt_Name;
        public Text Txt_Level;

        // Start is called before the first frame update
        void Start()
        {
            //Txt_Name.text = this.SlotType.ToString();
            //Txt_Level.text = "";
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
    }
}
