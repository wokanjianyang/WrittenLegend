using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Spirit : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Layer;
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Count;

        public SpiritConfig Config { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void OnEnable()
        {
            if (Config != null)
            {
                this.Show();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Dialog_Spirit dialog = this.GetComponentInParent<Dialog_Spirit>();
            dialog.ShowForge(Config.Id);
        }

        public void Show()
        {
            User user = GameProcessor.Inst.User;

            long spiritLevel = user.GetSpiritLevel(Config.Id);

            long total = user.GetItemMeterialCount(Config.ItemId);


            Txt_Level.text = spiritLevel + "级";
            Txt_Count.text = "拥有：" + total;
        }

        public void SetContent(SpiritConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), config.Name);

            this.Show();
        }
    }
}
