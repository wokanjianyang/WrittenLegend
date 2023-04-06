using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class Item_Skill : MonoBehaviour, IPointerClickHandler
    {
        [Title("技能")]
        [LabelText("名称")]
        public TextMeshProUGUI tmp_Name;

        [LabelText("等级")]
        public TextMeshProUGUI tmp_Level;

        [LabelText("冷却")]
        public TextMeshProUGUI tmp_CD;

        [LabelText("描述")]
        public TextMeshProUGUI tmp_Des;

        public SkillPanel SkillPanel { get; private set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetItem(SkillPanel skillPanel)
        {
            this.SkillPanel = skillPanel;

            if (SkillPanel.SkillData.SkillConfig.Name.Length > 2)
            {
                this.tmp_Name.text = SkillPanel.SkillData.SkillConfig.Name.Insert(2, "\n");
            }
            else
            {
                this.tmp_Name.text = SkillPanel.SkillData.SkillConfig.Name;
            }
            this.tmp_Level.text = string.Format("LV:{0}", SkillPanel.SkillData.Level);
            this.tmp_CD.text = string.Format("冷却时间{0}回合", SkillPanel.CD);

            this.tmp_Des.text = string.Format(SkillPanel.SkillData.SkillConfig.Des, SkillPanel.Dis, SkillPanel.EnemyMax, SkillPanel.Percent, SkillPanel.EnemyMax);

            var expProgress = this.GetComponentInChildren<Com_Progress>();
            expProgress.SetProgress(SkillPanel.SkillData.Exp, SkillPanel.SkillData.SkillConfig.Exp);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.SkillPanel == null || this.SkillPanel.SkillData == null) return;
            this.SkillPanel.SkillData.Status = SkillStatus.Equip;
            this.SkillPanel.SkillData.Position = (int)SkillPosition.Last; //
            GameProcessor.Inst.PlayerManager.GetHero().EventCenter.Raise(new HeroUpdateSkillEvent
            {
                SkillPanel = this.SkillPanel
            });
        }
    }
}
