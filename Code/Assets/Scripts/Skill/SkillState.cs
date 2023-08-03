using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillState
    {
        public SkillPanel SkillPanel { get; set; }
        public APlayer SelfPlayer { get; set; }

        public int Priority {  get; }
        public int lastUseRound { get; private set; } =0;

        public int UserCount { get; set; } = 0;

        public int Position { get; }

        private ASkill skillLogic;



        public SkillState(APlayer player, SkillPanel skillPanel, int position, int useRound)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skillPanel;
            this.Priority = position - skillPanel.SkillData.SkillConfig.Priority ;
            this.Position = position;
            this.lastUseRound = useRound;

            if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
            {
                if (skillPanel.SkillData.SkillConfig.CastType == ((int)AttackCastType.Single))
                {
                    this.skillLogic = new Skill_Attack_Single(player, skillPanel);
                }
                else
                {
                    this.skillLogic = new Skill_Attack_Area(player, skillPanel);
                }
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Valet)
            {
                this.skillLogic = new Skill_Valet(player, skillPanel);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Map)
            {
                this.skillLogic = new Skill_Attack_Map(player, skillPanel);
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
            else {
                this.skillLogic = new Skill_Attack_Normal(player, skillPanel);
            }
        }

        public bool IsCanUse()
        {
            return (this.lastUseRound == 0 || this.SelfPlayer.RoundCounter - lastUseRound > this.SkillPanel.CD) && this.skillLogic.IsCanUse();
        }

        public void Do()
        {
            this.lastUseRound = this.SelfPlayer.RoundCounter;
            this.skillLogic.Do();
        }

        public void SetLastUseRound(int round)
        {
            this.lastUseRound = round;
        }
    }
}
