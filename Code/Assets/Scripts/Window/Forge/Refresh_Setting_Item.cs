using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Refresh_Setting_Item : MonoBehaviour
    {
        public Text Txt_Title;
        public InputField If_Count;

        // Update is called once per frame
        void Start()
        {
        }

        public void SetItem(int attrId, int count)
        {
            Txt_Title.text = StringHelper.FormatAttrValueName(attrId);

            if (count > 0)
            {
                If_Count.text = count + "";
            }
            else
            {
                If_Count.text = "";
            }
        }
    }
}
