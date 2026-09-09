using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Abyss : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Layer;
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Count;

        public AbyssConfig Config { get; set; }

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
            Dialog_Abyss dialog = this.GetComponentInParent<Dialog_Abyss>();
            dialog.ShowForge(Config.Id);
        }

        public void Show()
        {
            User user = GameProcessor.Inst.User;

            AbyssData data = user.GetAbyss(Config.Id);

            Txt_Level.text = data.GetLevel() + "级";
            Txt_Count.text = "拥有：" + data.Total.Data;
        }

        public void Refresh(int type)
        {
            if (this.Config.Type != type)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }

        public void SetContent(AbyssConfig config)
        {
            this.Config = config;
            this.Txt_Name.text = string.Format("<color=#{0}>{1}</color>", QualityConfigHelper.GetQualityColor(Config.Quality), config.Name);

            this.Show();
        }
    }
}
