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

        public int AttrId = 0;

        // Update is called once per frame
        void Start()
        {
        }

        public void SetItem(int attrId)
        {
            this.AttrId = attrId;

            Txt_Title.text = StringHelper.FormatAttrValueName(attrId);
            If_Count.text = "";
        }

        public int GetCount()
        {
            int.TryParse(If_Count.text, out int exp);
            return exp;
        }
    }
}
