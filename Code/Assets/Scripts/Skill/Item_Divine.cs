using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Divine : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public SkillDivineConfig Config;
        private long Level;

        // Start is called before the first frame update
        void Awake()
        {
        }

        // Update is called once per frame
        void OnEnable()
        {

        }

        public void Init(ToggleGroup toggleGroup, SkillDivineConfig config)
        {
            toggle.group = toggleGroup;

            this.Config = config;

            Txt_Name.text = string.Format("<color=#888888>{0}</color>", Config.Name); ;
            Txt_Level.text = "";
        }

        public void SetContent(long level)
        {
            this.Level = level;

            if (level > 0)
            {
                Txt_Name.text = string.Format("<color=#FF0000>{0}</color>", Config.Name);
                Txt_Level.gameObject.SetActive(true);
                Txt_Level.text = level + "½×";
            }
            else
            {

                Txt_Name.text = string.Format("<color=#888888>{0}</color>", Config.Name);
                Txt_Level.gameObject.SetActive(false);
                Txt_Level.text = "";
            }
        }
    }
}
