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

        [LabelText("距离")]
        public Text txt_Dis;

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

        private void Init()
        {
            if (runeList.Count > 0)
            {
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

            User user = GameProcessor.Inst.User;

            if (this.SkillPanel.SkillData.MagicLevel.Data >= user.GetSkillLimit(this.SkillPanel.SkillData.SkillConfig))
            {
                this.Btn_UpLevel.gameObject.SetActive(false);
            }

            Recovery.isOn = skillPanel.SkillData.Recovery;

            this.tmp_Level.text = string.Format("LV:{0}", SkillPanel.SkillData.MagicLevel.Data);
            this.tmp_CD.text = string.Format("CD：{0}秒", SkillPanel.CD);
            this.txt_Dis.text = SkillPanel.Dis > 0 ? string.Format("施法距离：{0}格", SkillPanel.Dis) : "施法距离：无";
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

            List<int> list = user.GetCurrentSkillList();
            List<SkillData> skillList = user.SkillList.FindAll(m => list.Contains(m.SkillId));

            if (this.SkillPanel == null || this.SkillPanel.SkillData == null || skillList.Count >= user.SkillNumber)
            {
                return;
            }

            if (list.Contains(this.SkillPanel.SkillId))
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

            list.Add(this.SkillPanel.SkillId);
            GameProcessor.Inst.User.EventCenter.Raise(new SkillUpEvent());
        }

        public void Click_UpLevel()
        {
            int upCount = 20;

            int metailId = this.SkillPanel.SkillData.SkillConfig.UpItemId;
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(metailId);

            User user = GameProcessor.Inst.User;
            long total = user.Bags.Where(m => m.Item.Type == ItemType.Material && m.Item.ConfigId == metailId).Select(m => m.MagicNubmer.Data).Sum();

            //Debug.Log("max skill level:" + user.GetSkillLimit(this.SkillPanel.SkillData.SkillConfig));

            if (total < upCount)
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = itemConfig.Name + "数量不足" + upCount + "个", ToastType = ToastTypeEnum.Failure });
                return;
            }

            SkillData skill = this.SkillPanel.SkillData;

            if (skill.MagicLevel.Data >= user.GetSkillLimit(this.SkillPanel.SkillData.SkillConfig))
            {
                GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "技能已经满级了" });
                return;
            }

            GameProcessor.Inst.EventCenter.Raise(new SystemUseEvent()
            {
                Type = ItemType.Material,
                ItemId = metailId,
                Quantity = upCount
            });

            skill.MagicLevel.Data++;
            SkillPanel skillPanel = new SkillPanel(skill, user.GetRuneList(skill.SkillId, null), user.GetSuitList(skill.SkillId), true);
            this.SetItem(skillPanel);


            GameProcessor.Inst.EventCenter.Raise(new ShowGameMsgEvent() { Content = "消耗" + upCount + "个" + itemConfig.Name + "升级成功", ToastType = ToastTypeEnum.Success });
        }
    }
}
