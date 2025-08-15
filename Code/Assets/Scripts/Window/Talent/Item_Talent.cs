using Game.Data;
using Sirenix.OdinInspector;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Talent : MonoBehaviour, IPointerClickHandler
    {
        public Image Img_Bg;
        public Text Txt_Name;
        public Text Txt_Level;

        public TalentConfig Config { get; set; }

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Show()
        {
            if (this.Config == null)
            {
                return;
            }

            User user = GameProcessor.Inst.User;

            long level = user.GetTalentLevel(Config.Id);

            if (level > 0)
            {
                this.Txt_Level.gameObject.SetActive(true);
                this.Txt_Level.text = $"{level}/{Config.MaxLevel}";
            }
            else
            {
                this.Txt_Level.gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Config != null)
            {
                GameProcessor.Inst.EventCenter.Raise(new TalentDetailShowEvent() { Tid = Config.Id });
            }
        }

        public void SetContent(int tid)
        {
            if (!TalentConfigCategory.Instance.Contain(tid))
            {
                this.gameObject.SetActive(false);
                return;
            }
            else
            {
                this.gameObject.SetActive(true);
            }
            TalentConfig config = TalentConfigCategory.Instance.Get(tid);

            Texture2D texture = Resources.Load<Texture2D>("UI/Talent/" + config.Logo);
            Img_Bg.sprite = Sprite.Create(texture, new Rect(0, 0, 160, 160), Vector2.zero);


            this.Config = config;
            this.Txt_Name.text = config.Name;
            this.Show();
        }
    }
}
