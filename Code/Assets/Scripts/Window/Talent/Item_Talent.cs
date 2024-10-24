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
        public Text Txt_Name;
        public Text Txt_Level;

        public TalentConfig Config { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            if (Config != null)
            {
                this.Show();
            }
        }

        // Update is called once per frame
        void OnEnable()
        {

        }

        public void Show()
        {
            User user = GameProcessor.Inst.User;

            long level = user.GetTalentLevel(Config.Id);

            if (level > 0)
            {
                this.Txt_Level.gameObject.SetActive(false);
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
            this.Config = config;
            this.Txt_Name.text = config.Name;
            this.Show();
        }
    }
}
