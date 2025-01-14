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

        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public Pet pet;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnEnable()
        {

        }

        public void Init(Pet pet)
        {
            this.pet = pet;

            Txt_Name.text = pet.Name;
            Txt_Level.text = pet.PetLevel.Data + "¼¶";

            this.image_Background.sprite = list_Backgrounds[pet.Role - 1];
        }
    }
}