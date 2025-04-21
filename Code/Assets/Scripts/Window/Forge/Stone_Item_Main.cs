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
    public class StoneMainSelectEvent : UnityEvent<int> { } // 支持int和string参数

    public class Stone_Item_Main : MonoBehaviour
    {
        public Text Txt_Level;
        public Toggle toggle;

        public Image image_Background;
        public Sprite[] list_Backgrounds;

        private int MainIndex { get; set; } = 0;
        private int StoneId { get; set; } = 0;
        private int StoneLevel { get; set; } = 0;


        [SerializeField]
        private StoneMainSelectEvent _onValueChanged = new StoneMainSelectEvent();

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
            Debug.Log("Stone_Item_Main Show Index:" + MainIndex);


            this.Txt_Level.gameObject.SetActive(false);

            if (this.StoneId == 0)
            {
                this.image_Background.sprite = list_Backgrounds[0];
                return;
            }
            else
            {
                this.image_Background.sprite = list_Backgrounds[StoneId];

                if (StoneLevel > 0)
                {
                    this.Txt_Level.gameObject.SetActive(true);
                    this.Txt_Level.text = StoneLevel + "";
                }
            }
            //this.Txt_Name.text = Config.Name.Insert(2, "\n"); ;

            //User user = GameProcessor.Inst.User;
            //int level = user.GetRelicLevel(Config.Id);
            //this.Txt_Level.text = level + "";
        }

        private void Select()
        {
            if (toggle.isOn)
            {
                _onValueChanged.Invoke(MainIndex);
            }
        }


        public void SetContent(int index, int stoneId, int stoneLevel)
        {
            this.MainIndex = index;
            this.StoneId = stoneId;
            this.StoneLevel = stoneLevel;

            this.Show();
        }
    }
}
