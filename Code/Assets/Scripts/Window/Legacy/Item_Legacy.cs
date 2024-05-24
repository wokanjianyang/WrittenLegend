using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Legacy : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Layer;
        public Text Txt_Level;
        public Toggle toggle;

        public LegacyConfig Config;
        private long Level;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Name.text = "";
            Txt_Level.text = "";
            Txt_Layer.text = "";
        }

        // Update is called once per frame
        void OnEnable()
        {

        }

        public void Init(ToggleGroup toggleGroup)
        {
            toggle.group = toggleGroup;
        }

        public void Change(LegacyConfig config)
        {
            this.Config = config;

            Txt_Name.text = config.Name;
            Txt_Level.text = "";
            Txt_Layer.text = "";
        }

        public void SetContent(long layer, long level)
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

            if (layer > 0)
            {
                Txt_Layer.text = layer + "½×";
            }
            else
            {
                Txt_Layer.text = "";
            }
        }
    }
}
