using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Metal : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Attr_Current;
        public Text Txt_Attr_Rise;
        public Text Txt_RisePower;

        public MetalConfig Config { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void SetContent(MetalConfig config, long level, long percent)
        {
            this.Config = config;

            if (level <= 0)
            {
                this.gameObject.SetActive(false);
                return;
            }
            else
            {
                this.gameObject.SetActive(true);
            }

            long riseLevel = level * percent / 100;
            riseLevel = Math.Max(riseLevel, percent);

            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), config.Name);
            this.Txt_Level.text = $"{level}+{riseLevel}个";


            long rate = this.Config.GetRate(level + riseLevel);
            long val = config.GetAttr(level + riseLevel);
            long nr = config.GetNextRate(level + riseLevel);

            if (config.AttrId > 0)
            {
                this.Txt_Attr_Current.text = StringHelper.FormatAttrText(config.AttrId, val);
                this.Txt_Attr_Rise.text = "单个属性:" + StringHelper.FormatAttrValueText(config.AttrId, config.AttrValue * rate);
            }
            else
            {
                this.Txt_Attr_Current.text = string.Format(config.Des, level);
                this.Txt_Attr_Rise.text = "单个属性:1%";
            }

            if (Config.RisePower > 0)
            {
                this.Txt_RisePower.gameObject.SetActive(true);
                this.Txt_RisePower.text = "满" + nr + "个" + "单个属性\n再提高" + config.RisePower + "倍";
            }
            else
            {
                this.Txt_RisePower.gameObject.SetActive(false);
            }
        }
    }
}
