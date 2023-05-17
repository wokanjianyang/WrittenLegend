using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillState
    {
        public SkillPanel SkillPanel { get; set; }
        public APlayer SelfPlayer { get; set; }

        public int Priority { get; set; }

        private ASkill skillLogic;

        private int lastUseRound = 0;

        public SkillState(APlayer player, SkillPanel skillPanel, int position,int useRound)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skillPanel;
            this.Priority = position - skillPanel.SkillData.SkillConfig.Priority;
            this.lastUseRound = useRound;

            if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
            {
                this.skillLogic = new Skill_Sweep(player, skillPanel);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Valet)
            {
                this.skillLogic = new Skill_Valet(player, skillPanel);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Map)
            {
                this.skillLogic = new Skill_Map(player, skillPanel);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Restore)
            {
                this.skillLogic = new Skill_Restore(player, skillPanel);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Shield)
            {
                this.skillLogic = new Skill_Shield(player, skillPanel);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Expert)
            {
                this.skillLogic = new Skill_Expert(player, skillPanel);
            }
            else
            {
                this.skillLogic = new BaseAttackSkill(player, skillPanel);
            }
        }

        public bool IsCanUse()
        {
            return (this.lastUseRound == 0 || this.SelfPlayer.RoundCounter - lastUseRound > this.SkillPanel.CD) && this.skillLogic.IsCanUse();
        }

        public List<AttackData> GetAllTarget()
        {
            return skillLogic.GetAllTargets();
        }

        public void Do(List<AttackData> targets)
        {
            this.lastUseRound = this.SelfPlayer.RoundCounter;

            foreach (AttackData attack in targets)
            {
                //this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent()
                //{
                //    TargetId = attack.Tid,
                //    Content = this.SkillPanel.SkillData.SkillConfig.Name
                //});
            }

            this.skillLogic.Do();
        }

        public void Destory() {
            if (skillLogic is Skill_Valet) {
                Skill_Valet sv = skillLogic as Skill_Valet;
                sv.ClearValet();
            }
        }
    }
}
