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
        public TextMeshProUGUI tmp_Name;

        [LabelText("技能")]
        public Button btn_Skill;

        public SkillData SkillData { get; private set; }

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
            this.SkillData.Status = SkillStatus.Learn;
            GameProcessor.Inst.PlayerManager.hero.EventCenter.Raise(new HeroUpdateSkillEvent
            {
                SkillData = this.SkillData
            });
        }

        public void SetItem(SkillData skill)
        {
            this.tmp_Name.text = skill.Name;

            this.SkillData = skill;

            this.tran_Skill.gameObject.SetActive(true);
        }
    }
}
