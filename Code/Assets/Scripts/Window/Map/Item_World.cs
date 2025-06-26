using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_World : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;

        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Button Btn_OK;

        WorldConfig Config;

        private int level = 0;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_OK.onClick.AddListener(OnClickOk);
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

        public void OnClickOk()
        {
            this.level = GameProcessor.Inst.User.WorldData.GetLayer(this.Config.Id);

            if (this.level > ConfigHelper.MaxWorld)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已通关，请等下次", ToastType = ToastTypeEnum.Failure });
                return;
            }

            var dialog = this.GetComponentInParent<Map_Dialog_World>();
            dialog.gameObject.SetActive(false);

            GameProcessor.Inst.World_Auto_Id = this.Config.Id;

            var vm = this.GetComponentInParent<ViewMore>();
            vm.StartWorld(Config.Id);
        }

        public void Show()
        {
            this.level = GameProcessor.Inst.User.WorldData.GetLayer(this.Config.Id);

            this.Txt_Name.text = Config.MapName;

            if (this.level <= ConfigHelper.MaxWorld)
            {
                this.Txt_Level.text = $"{level}级";
            }
            else
            {
                this.Txt_Level.text = "完美通关";
            }
        }

        public void SetContent(WorldConfig config)
        {
            this.Config = config;
            this.Show();

            this.image_Background.sprite = list_Backgrounds[Config.Id - 1];
        }
    }
}
