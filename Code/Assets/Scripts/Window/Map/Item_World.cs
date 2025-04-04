using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_World : MonoBehaviour, IPointerClickHandler
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Desc;

        WorldConfig Config;

        private int level = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void OnEnable()
        {
            if (this.Config == null)
            {
                return;
            }

            this.Show();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            this.level = GameProcessor.Inst.User.WorldData.GetLayer(this.Config.Id);

            if (this.level >= 100)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已通关，请等下个月", ToastType = ToastTypeEnum.Failure });
                return;
            }

            var dialog = this.GetComponentInParent<Map_Dialog_World>();
            dialog.gameObject.SetActive(false);

            var vm = this.GetComponentInParent<ViewMore>();
            vm.StartWorld(Config.Id, this.level);
        }

        public void Show()
        {
            this.level = GameProcessor.Inst.User.WorldData.GetLayer(this.Config.Id);

            this.Txt_Name.text = Config.MapName;
            this.Txt_Level.text = $"{level}(轮)"; ;
            this.Txt_Desc.text = Config.Desc;
        }

        public void SetContent(WorldConfig config)
        {
            this.Config = config;
            this.Show();
        }
    }
}
