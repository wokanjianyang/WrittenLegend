using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class StrenthAttrItem : MonoBehaviour
    {
        [LabelText("Txt_Name")]
        public Text Txt_Name;

        [LabelText("Txt_Attr")]
        public Text Txt_Attr;

        [LabelText("Txt_Attr_Add")]
        public Text Txt_Attr_Add;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetContent(int attrId, long attrBase, long percent, long attrRise)
        {
            this.Txt_Name.text = StringHelper.FormatAttrValueName(attrId);

            if (attrBase > 0)
            {
                string attrText = StringHelper.FormatAttrValueText(attrId, attrBase);
                if (percent > 0)
                {
                    long pb = attrBase / 100 * percent;
                    attrText += "(" + StringHelper.FormatAttrValueText(attrId, pb) + ")";
                }

                this.Txt_Attr.text = attrText;
            }
            else
            {
                this.Txt_Attr.text = "";
            }

            if (attrRise > 0)
            {
                this.Txt_Attr_Add.text = " + " + StringHelper.FormatAttrValueText(attrId, attrRise);
            }
            else
            {
                this.Txt_Attr_Add.text = "";
            }
        }

        public void SetContent(int attrId, long attrBase, long attrRise)
        {
            this.SetContent(attrId, attrBase, 0, attrRise);
        }
    }
}
