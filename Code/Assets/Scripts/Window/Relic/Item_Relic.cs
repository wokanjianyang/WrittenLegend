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
    public class RelicItemSelectEvent : UnityEvent<int> { } // 支持int和string参数

    public class Item_Relic : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Level;
        public Toggle toggle;

        public RelicConfig Config { get; set; }


        [SerializeField]
        private RelicItemSelectEvent _onValueChanged = new RelicItemSelectEvent();

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
            int level = user.GetRelicLevel(Config.Id);
            int rise = user.GetRelicRise();

            this.Txt_Level.text = rise > 0 ? level + "+" + rise : level + "";
        }

        private void Select()
        {
            if (toggle.isOn)
            {
                _onValueChanged.Invoke(Config.Id);
            }
        }


        public void SetContent(RelicConfig config)
        {
            this.Config = config;

            this.Show();
        }
    }
}
