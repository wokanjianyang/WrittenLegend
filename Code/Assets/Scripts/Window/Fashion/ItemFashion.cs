using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Game
{
    public class ItemFashion : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public int Part;
        public FashionConfig Config;
        private long Level;

        // Start is called before the first frame update
        void Awake()
        {
            Txt_Name.text = "";
            Txt_Level.text = "";
        }

        // Update is called once per frame
        void OnEnable()
        {
            if (Config != null)
            {
                Check();
            }
        }

        private void Check()
        {
            User user = GameProcessor.Inst.User;
            long total = user.GetItemMeterialCount(Config.ItemId);
            int needCount = CalNeedCount((int)Level);
            string color = total >= needCount ? "#FFFF00" : "#FF0000";
            Txt_Name.text = string.Format("<color={0}>{1}</color>", color, Config.Name);
        }

        private int CalNeedCount(int currentLevel)
        {
            return Math.Min(currentLevel + 1, 20);
        }

        public void Init(int part, FashionConfig config)
        {
            this.Part = part;
            this.Config = config;

            Txt_Name.text = config.Name;
            Txt_Level.text = "";
        }

        public void SetLevel(long level)
        {
            this.Level = level;
            if (level > 0)
            {
                Txt_Level.text = level + "";
            }
            else
            {
                Txt_Level.text = "";
            }

            Check();
        }
    }
}
