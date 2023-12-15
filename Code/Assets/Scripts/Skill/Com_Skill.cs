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
            Clear();

            User user = GameProcessor.Inst.User;

            List<int> list = user.GetCurrentSkillList();
            list.Remove(this.SkillData.SkillId);

            GameProcessor.Inst.User.EventCenter.Raise(new SkillDownEvent());
        }

        public void SetItem(SkillData skillData)
        {
            this.SkillData = skillData;

            if (SkillData.SkillConfig.Name.Length > 2)
            {
                this.tmp_Name.text = SkillData.SkillConfig.Name.Insert(2, "\n");
            }
            else
            {
                this.tmp_Name.text = SkillData.SkillConfig.Name;
            }

            this.tran_Skill.gameObject.SetActive(true);
        }

        public void Clear()
        {
            this.tran_Skill.gameObject.SetActive(false);
        }
    }
}
