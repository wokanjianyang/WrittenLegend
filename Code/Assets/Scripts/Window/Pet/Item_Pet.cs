using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Pet : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Text Txt_Layer;

        public Button Btn_Down;
        public Button Btn_Up_Level;
        public Button Btn_Travel;
        public Button Btn_Devour;

        public Button Btn_Image;
        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Pet pet;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Image.onClick.AddListener(ShowDetail);
            this.Btn_Down.onClick.AddListener(OnDown);
            this.Btn_Up_Level.onClick.AddListener(OnUpLevel);
            this.Btn_Travel.onClick.AddListener(OnTravel);
            this.Btn_Devour.onClick.AddListener(OnDevour);
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
            box.Item = pet;
            box.BoxId = -1;

            GameProcessor.Inst.EventCenter.Raise(new ShowPetDetailEvent()
            {
                boxItem = box,
            });
        }

        private void OnDown()
        {
            if (pet.RunMapId > 0)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "巡游中不可以下阵", ToastType = ToastTypeEnum.Failure });
                return;
            }

            this.gameObject.gameObject.SetActive(false);

            GameProcessor.Inst.EventCenter.Raise(new PetBattleDownEvent()
            {
                Item = this
            });
        }

        private void OnUpLevel()
        {
            GameProcessor.Inst.EventCenter.Raise(new OpenPetForgeEvent() { Type = 1, Item = this });
        }

        private void OnTravel()
        {
            GameProcessor.Inst.EventCenter.Raise(new OpenPetForgeEvent() { Type = 2, Item = this });
        }

        private void OnDevour()
        {
            GameProcessor.Inst.EventCenter.Raise(new OpenPetForgeEvent() { Type = 3, Item = this });
        }

        public void Init(Pet pet)
        {
            this.pet = pet;

            Txt_Name.text = pet.Name;
            Txt_Level.text = pet.PetLevel.Data + "级";
            Txt_Layer.text = pet.PetLayer.Data + "阶";

            Txt_Level.color = ColorHelper.GetColorByQuality(pet.GetQuality());
            Txt_Layer.color = ColorHelper.GetColorByQuality(pet.GetQuality());

            this.image_Background.sprite = list_Backgrounds[pet.Role - 1];

            if (pet.GetQuality() >= 7 && pet.DevourFlairs.Count == 0 && pet.PetLayer.Data >= 10)
            {
                this.Btn_Devour.gameObject.SetActive(true);
            }
            else
            {
                this.Btn_Devour.gameObject.SetActive(false);
            }
        }
    }
}