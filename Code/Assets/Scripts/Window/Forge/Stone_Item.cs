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
    public class StoneItemSelectEvent : UnityEvent<int> { } // 支持int和string参数

    public class Stone_Item : MonoBehaviour
    {
        public Text Txt_Count;
        public Toggle toggle;

        public Image image_Background;
        public Sprite[] list_Backgrounds;

        public StoneConfig Config { get; set; }


        [SerializeField]
        private StoneItemSelectEvent _onValueChanged = new StoneItemSelectEvent();

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
            if (Config != null)
            {
                this.Show();
            }
        }

        public void AddListener(UnityAction<int> callback)
        {
            _onValueChanged.AddListener(callback);
        }

        public void Show()
        {
            //Debug.Log("item relic show");

            if (this.Config == null)
            {
                return;
            }

            User user = GameProcessor.Inst.User;
            long count = user.GetMaterialCount(Config.ItemId);
            this.Txt_Count.text = count + "";
        }

        private void Select()
        {
            if (toggle.isOn)
            {
                _onValueChanged.Invoke(Config.Id);
            }
        }


        public void SetContent(StoneConfig config)
        {
            this.Config = config;

            this.image_Background.sprite = list_Backgrounds[config.Id - 1];
            this.Show();
        }
    }
}
