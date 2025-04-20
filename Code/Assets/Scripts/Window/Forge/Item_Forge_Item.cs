using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    [Serializable]
    public class ForgeItemEvent : UnityEvent<int> { } // 支持int和string参数

    public class Item_Forge_Item : MonoBehaviour
    {
        public Text Txt_Name;
        public Toggle toggle;

        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public int Id { get; set; }


        [SerializeField]
        private ForgeItemEvent _onValueChanged = new ForgeItemEvent();

        // Start is called before the first frame update
        void Start()
        {
            toggle.onValueChanged.AddListener((isOn) =>
            {
                this.Select();
            });
        }

        // Update is called once per frame
        void OnEnable()
        {
            //if (Config != null)
            //{
            //    this.Show();
            //}
        }

        public void AddListener(UnityAction<int> callback)
        {
            _onValueChanged.AddListener(callback);
        }

        public void Show()
        {
            //Debug.Log("item relic show");
        }

        private void Select()
        {
            if (toggle.isOn)
            {
                _onValueChanged.Invoke(this.Id);
            }
        }


        public void SetContent(int index, int id)
        {
            this.Id = id;
            this.image_Background.sprite = list_Backgrounds[index - 1];

            this.Show();
        }
    }
}
