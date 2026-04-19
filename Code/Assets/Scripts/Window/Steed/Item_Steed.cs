using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Steed : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Layer;

        public Button Btn_Down;
        public Button Btn_Up_Level;
        public Button Btn_Travel;

        public Button Btn_Image;
        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Steed steed;
        private int PetCycle = 1;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Image.onClick.AddListener(ShowDetail);
            this.Btn_Down.onClick.AddListener(OnDown);
            this.Btn_Up_Level.onClick.AddListener(OnUpLevel);
            this.Btn_Travel.onClick.AddListener(OnTravel);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {

        }

        private void ShowDetail()
        {
            BoxItem box = new BoxItem();
            box.Item = steed;
            box.BoxId = -1;

            GameProcessor.Inst.EventCenter.Raise(new ShowPetDetailEvent()
            {
                boxItem = box,
                Type = ComBoxType.Gift
            });
        }

        private void OnDown()
        {
            if (steed.RunMapId > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "巡游中不可以下阵", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.gameObject.SetActive(false);


            GameProcessor.Inst.EventCenter.Raise(new SteedBattleDownEvent() { Item = this });

        }

        private void OnUpLevel()
        {
            GameProcessor.Inst.EventCenter.Raise(new SteedOpenForgeEvent() { Type = 1, Item = this });
        }

        private void OnTravel()
        {
            GameProcessor.Inst.EventCenter.Raise(new SteedOpenForgeEvent() { Type = 2, Item = this });
        }

        public void Init(Steed sd)
        {
            this.steed = sd;

            Txt_Name.text = steed.Name;
            Txt_Level.text = steed.PetLevel.Data + "级";
            Txt_Layer.text = steed.PetLayer.Data + "阶";

            Txt_Level.color = ColorHelper.GetColorByQuality(steed.GetQuality());
            Txt_Layer.color = ColorHelper.GetColorByQuality(steed.GetQuality());

            this.PetCycle = steed.GetQuality() == 9 ? 2 : 1;

            this.image_Background.sprite = list_Backgrounds[steed.Role - 1 + PetCycle * 3 - 3];
        }
    }
}