using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Item_Skill_Rune : MonoBehaviour
    {
        public Text Txt_Name;
        public Text Txt_Count;
        public Text Txt_Desc;
        public Button Btn_Show;

        public Image Img_Bg;

        private bool active = false;

        // Start is called before the first frame update
        void Start()
        {
            this.Btn_Show.onClick.AddListener(this.OnClick_Show);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnClick_Show()
        {
            active = !active;
            Img_Bg.gameObject.SetActive(active);
            Txt_Desc.gameObject.SetActive(active);
        }

        public void SetRune(int runeId, int count)
        {
            SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(runeId);

            string name = config.Name;
            if (name.Contains("·"))
            {
                name = name.Substring(name.IndexOf("·") + 1);
            }
            Txt_Name.text = name + "：";

            Txt_Count.text = count > 0 ? "+" + count + "" : "无";

            Txt_Desc.text = config.Des;
        }

        public void SetSuit(int suitId, int count)
        {
            SkillSuitConfig config = SkillSuitConfigCategory.Instance.Get(suitId);

            string name = config.Name;
            if (name.Contains("·"))
            {
                name = name.Substring(name.IndexOf("·") + 1);
            }
            Txt_Name.text = name + "：";

            Txt_Count.text = count > 0 ? "+" + count + "" : "无";

            Txt_Desc.text = config.Des;
        }
    }
}
