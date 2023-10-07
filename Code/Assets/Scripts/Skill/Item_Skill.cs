using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Item_Skill : MonoBehaviour, IPointerClickHandler
    {
        [Title("物品")]
        [LabelText("名称")]
        public Text tmp_Name;

        [LabelText("等级")]
        public Text tmp_Level;

        [LabelText("冷却")]
        public Text tmp_CD;

        [LabelText("描述")]
        public Text tmp_Des;

        public Toggle Recovery;

        public Button Btn_UpLevel;

        List<Text> runeList = new List<Text>();
        List<Text> suitList = new List<Text>();
        public SkillPanel SkillPanel { get; private set; }

        // Start is called before the first frame update
        void Start()
        {

            Recovery.onValueChanged.AddListener((isOn) =>
            {
                this.SkillPanel.SkillData.Recovery = isOn;
            });


            this.Btn_UpLevel.onClick.AddListener(this.Click_UpLevel);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Init() {
            if (runeList.Count > 0) {
                return;
            }

            for (int i = 1; i <= 8; i++)
            {
                Text text = this.transform.Find(string.Format("Txt_Rune{0}", i)).GetComponent<Text>();
                if (text != null)
                {
                    runeList.Add(text);
                }
            }

            for (int i = 1; i <= 4; i++)
            {
                Text text = this.transform.Find(string.Format("Txt_Suit{0}", i)).GetComponent<Text>();
                if (text != null)
                {
                    suitList.Add(text);
                }
            }
        }

        public void SetItem(SkillPanel skillPanel)
        {
            this.Init();
            this.SkillPanel = skillPanel;

            if (SkillPanel.SkillData.SkillConfig.Name.Length > 2)
            {
                this.tmp_Name.text = SkillPanel.SkillData.SkillConfig.Name.Insert(2, "\n");
            }
            else
            {
                this.tmp_Name.text = SkillPanel.SkillData.SkillConfig.Name;
            }

            for (int i = 0; i < runeList.Count; i++)
            {
                if (i < skillPanel.RuneTextList.Count)
                {
                    runeList[i].gameObject.SetActive(true);
                    runeList[i].text = formatText(skillPanel.RuneTextList[i]);
                }
                else
                {
                    runeList[i].gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < suitList.Count; i++)
            {
                if (i < skillPanel.SuitTextList.Count)
                {
                    suitList[i].gameObject.SetActive(true);
                    suitList[i].text = formatText(skillPanel.SuitTextList[i]);
                }
                else
                {
                    suitList[i].gameObject.SetActive(false);
                }
            }

            if (this.SkillPanel.SkillData.MagicLevel.Data >= this.SkillPanel.SkillData.SkillConfig.MaxLevel)
            {
                this.Btn_UpLevel.gameObject.SetActive(false);
            }

            Recovery.isOn = skillPanel.SkillData.Recovery;

            this.tmp_Level.text = string.Format("LV:{0}", SkillPanel.SkillData.MagicLevel.Data);
            this.tmp_CD.text = string.Format("冷却时间{0}秒", SkillPanel.CD);

            this.tmp_Des.text = SkillPanel.Desc;

            var expProgress = this.GetComponentInChildren<Com_Progress>();
            expProgress.SetProgress(SkillPanel.SkillData.MagicExp.Data, SkillPanel.SkillData.GetLevelUpExp());
        }

        private string formatText(KeyValuePair<string, int> kp)
        {
            string name = kp.Key;
            if (name.Contains("·"))
            {
                name = name.Substring(name.IndexOf("·") + 1);
            }
            //Debug.Log("name:" + name);

            string ct = kp.Value > 0 ? "+" + kp.Value : "无";
            return name + "：" + string.Format("<color=#FF0000>{0}</color>", ct);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            User user = GameProcessor.Inst.User;
            List<SkillData> skillList = user.SkillList.FindAll(m => m.Status == SkillStatus.Equip);

            if (this.SkillPanel == null || this.SkillPanel.SkillData == null || skillList.Count >=  user.SkillNumber)
            {
                return;
            }

            if (this.SkillPanel.SkillData.Status == SkillStatus.Equip)
            {
                return;
            }

            int repet = this.SkillPanel.SkillData.SkillConfig.Repet;
            if (repet > 0)
            {
                //查找是否已经上阵了同类技能
                if (skillList.Where(m => m.SkillConfig.Repet == repet).Count() > 0)
                {
                    GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "已经上阵了同类技能", ToastType = ToastTypeEnum.Failure });
                    return;
                }
            }

            this.SkillPanel.SkillData.Status = SkillStatus.Equip;
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent() { SkillPanel = this.SkillPanel });
        }

        public void Click_UpLevel()
        {
            int upCount = 20;

            User user = GameProcessor.Inst.User;
            long total = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == ItemHelper.SpecialId_Moon_Cake).Select(m => m.MagicNubmer.Data).Sum();

            if (total < upCount)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "月饼数量不足20个" });
                return;
            }

            SkillData skill = this.SkillPanel.SkillData;

            if (skill.MagicLevel.Data >= skill.SkillConfig.MaxLevel)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "技能已经满级了" });
                return;
            }

            skill.MagicLevel.Data++;

            SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId), user.GetSuitList(skill.SkillId), true);
            this.SetItem(skillPanel);

            GameProcessor.Inst.EventCenter.Raise(new MaterialUseEvent()
            {
                MaterialId = ItemHelper.SpecialId_Moon_Cake,
                Quantity = upCount
            });

        }
    }
}
