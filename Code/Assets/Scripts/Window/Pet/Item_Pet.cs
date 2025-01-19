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

        public Button Btn_Down;
        public Button Btn_Up_Level;
        public Button Btn_Travel;

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

            GameProcessor.Inst.EventCenter.Raise(new PetBattleDownEvent()
            {
                Item = this
            });
        }

        private void OnUpLevel()
        {

            GameProcessor.Inst.EventCenter.Raise(new PetForgeEvent()
            {
                Item = this
            });
        }

        private void OnTravel() {
            GameProcessor.Inst.EventCenter.Raise(new PetOpenTravelEvent()
            {
                Pet = pet
            });

        }

        public void Init(Pet pet)
        {
            this.pet = pet;

            Txt_Name.text = pet.Name;
            Txt_Level.text = pet.PetLevel.Data + "¼¶";
            Txt_Level.color = ColorHelper.GetColorByQuality(pet.GetQuality());

            this.image_Background.sprite = list_Backgrounds[pet.Role - 1];
        }
    }
}