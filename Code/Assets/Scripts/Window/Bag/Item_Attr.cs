using Game.Data;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Attr : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Value;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetContent(int attrId, double attrValue)
        {
            Txt_Name.text = StringHelper.FormatAttrValueName(attrId);
            Txt_Value.text = StringHelper.FormatAttrValueText(attrId,attrValue);
        }
    }
}
