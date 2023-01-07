using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
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

        public SkillData SkillData { get; private set; }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetItem(SkillData skillData)
        {
            this.SkillData = skillData;
            if (skillData.Name.Length > 2)
            {
                this.tmp_Name.text = skillData.Name.Insert(2, "\n");
            }
            else
            {
                this.tmp_Name.text = SkillData.Name;
            }
            this.tmp_Level.text = string.Format("LV:{0}", SkillData.Level);
            this.tmp_CD.text = string.Format("冷却时间{0}回合", SkillData.CD);
            this.tmp_Des.text = SkillData.Des;

            var expProgress = this.GetComponentInChildren<Com_Progress>();
            expProgress.SetProgress(SkillData.Exp, SkillData.UpExp);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.SkillData == null) return;
            this.SkillData.Status = SkillStatus.Equip;
            GameProcessor.Inst.PlayerManager.GetHero().EventCenter.Raise(new HeroUpdateSkillEvent
            {
                SkillData = this.SkillData
            });
        }

    }
}
