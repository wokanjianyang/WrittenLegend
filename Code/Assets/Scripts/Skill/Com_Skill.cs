using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Com_Skill : MonoBehaviour
    {
        [Title("技能")]
        [LabelText("技能")]
        public Transform tran_Skill;

        [LabelText("名称")]
        public Text tmp_Name;

        [LabelText("移除")]
        public Button btn_Skill;

        public SkillPanel SkillPanel { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
            this.btn_Skill.onClick.AddListener(this.OnClick_RemoveSkill);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnClick_RemoveSkill()
        {
            this.tran_Skill.gameObject.SetActive(false);

            this.SkillPanel.SkillData.Status = SkillStatus.Learn;
            this.SkillPanel.SkillData.Position = 0;
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent() { SkillPanel = this.SkillPanel });
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

            this.tran_Skill.gameObject.SetActive(true);
        }
    }
}
