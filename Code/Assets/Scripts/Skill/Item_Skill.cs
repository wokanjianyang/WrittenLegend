using Sirenix.OdinInspector;
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
            this.tmp_CD.text = string.Format("冷却时间{0}秒", SkillPanel.CD);

            this.tmp_Des.text = string.Format(SkillPanel.SkillData.SkillConfig.Des, SkillPanel.Dis, SkillPanel.EnemyMax, SkillPanel.Percent, SkillPanel.Damage);

            var expProgress = this.GetComponentInChildren<Com_Progress>();
            expProgress.SetProgress(SkillPanel.SkillData.Exp, SkillPanel.SkillData.GetLevelUpExp());
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            int count = GameProcessor.Inst.User.SkillList.FindAll(m => m.Status == SkillStatus.Equip).Count;
            if (this.SkillPanel == null || this.SkillPanel.SkillData == null || count >= 5)
            {
                return;
            }

            if (this.SkillPanel.SkillData.Status == SkillStatus.Equip)
            {
                return;
            }

            this.SkillPanel.SkillData.Status = SkillStatus.Equip;
            GameProcessor.Inst.User.EventCenter.Raise(new HeroUpdateSkillEvent() { SkillPanel = this.SkillPanel });
        }
    }
}
