using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class RefineAttrItem : MonoBehaviour
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

        public void SetContent(string name,string attr,string attr_add) {
            this.Txt_Name.text = name;
            this.Txt_Attr.text = attr;
            this.Txt_Attr_Add.text = attr_add;
        }

    }
}
