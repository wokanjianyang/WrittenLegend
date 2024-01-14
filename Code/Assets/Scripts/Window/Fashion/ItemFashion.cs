using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class ItemFashion : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public int Part;
        public FashionConfig Config;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Name.text = "";
            Txt_Level.text = "";
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(int part,FashionConfig config)
        {
            this.Part = part;
            this.Config = config;

            Txt_Name.text = config.Name;
            Txt_Level.text = "";
        }

        public void SetLevel(long level)
        {
            if (level > 0)
            {
                Txt_Level.text = level + "";
            }
            else
            {
                Txt_Level.text = "";
            }
        }
    }
}
