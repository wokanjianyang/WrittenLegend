using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Ring : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public RingConfig Config;
        private long Level;

        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void OnEnable()
        {

        }

        public void Init(ToggleGroup toggleGroup, RingConfig config)
        {
            toggle.group = toggleGroup;

            this.Config = config;

            Txt_Name.text = config.Name;
            Txt_Level.text = "";
        }

        public void SetContent(long level)
        {
            this.Level = level;

            if (level > 0)
            {
                Txt_Level.text = level + "¼¶";
            }
            else
            {
                Txt_Level.text = "";
            }
        }
    }
}
