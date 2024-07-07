using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Box_Ready : MonoBehaviour
    {
        public Text Txt_Name;

        public Box_Select BoxSelect;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(string name)
        {
            Txt_Name.text = name;
        }

        public void Up(BoxItem boxItem)
        {
            Txt_Name.gameObject.SetActive(false);
            BoxSelect.SetItem(boxItem, ComBoxType.Box_Ready);
            BoxSelect.gameObject.SetActive(true);
        }

        public void Down()
        {
            Txt_Name.gameObject.SetActive(true);
            BoxSelect.gameObject.SetActive(false);
        }
    }
}
