using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Copy_Legacy : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;

        public List<Text> TextPowerList;

        public Button Btn_Start;

        private LegacyMapConfig Config { get; set; }
        private long Layer = 0;
        private string[] PowerNameList = new string[] { "天之力", "地之力", "人之力" };

        // Start is called before the first frame update
        void Start()
        {
            Btn_Start.onClick.AddListener(OnClick_Start);
        }

        // Update is called once per frame
        void OnEnable()
        {
            this.Show();
        }

        public void Init(LegacyMapConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = config.Name;

            this.Show();
        }

        private void Show()
        {
            User user = GameProcessor.Inst.User;

            if (Config == null || user == null)
            {
                return;
            }

            long[] powerList = new long[] { 0, 0, 0 };


            foreach (KeyValuePair<int, MagicData> kv in user.LegacyLayer)
            {
                LegacyConfig legacy = LegacyConfigCategory.Instance.Get(kv.Key);

                for (int i = 0; i < legacy.PowerList.Length; i++)
                {
                    powerList[i] += kv.Value.Data * legacy.PowerList[i];
                }
            }


            this.Layer = Config.CalMaxLayer(powerList);

            this.Txt_Level.text = $"{ Layer }阶";


            for (int i = 0; i < Config.PowerList.Length; i++)
            {
                long needNumber = Config.PowerList[i] * Layer;
                long total = powerList[i];

                string color = total >= needNumber ? "#00EE00" : "#EE0000";

                TextPowerList[i].text = PowerNameList[i] + "： " + string.Format("<color={0}>{1}</color> /{2}", color, total, needNumber);
            }
        }
        public void OnClick_Start()
        {
            var dialogLegacy = this.GetComponentInParent<Dialog_Copy_Legacy>();
            dialogLegacy.gameObject.SetActive(false);

            var vm = this.GetComponentInParent<ViewMore>();
            vm.StartLegacy(Config.Id, (int)Layer);
        }

    }
}
