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
    public class CardSpecialItemSelectEvent : UnityEvent<int> { } // 支持int和string参数

    public class Item_Card_Special : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public CardSpecialConfig Config { get; set; }


        [SerializeField]
        private CardSpecialItemSelectEvent _onValueChanged = new CardSpecialItemSelectEvent();

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

            if (this.Config == null)
            {
                return;
            }

            this.Txt_Name.text = Config.Name.Insert(2, "\n"); ;

            User user = GameProcessor.Inst.User;
            int level = user.GetCardSpecialLevel(Config.Id);
            this.Txt_Level.text = level + "";
        }

        private void Select()
        {
            if (toggle.isOn)
            {
                _onValueChanged.Invoke(Config.Id);
            }
        }


        public void SetContent(CardSpecialConfig config)
        {
            this.Config = config;

            this.Show();
        }
    }
}
