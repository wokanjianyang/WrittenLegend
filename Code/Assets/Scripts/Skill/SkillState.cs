using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillState
    {
        public SkillPanel SkillPanel { get; set; }
        public APlayer SelfPlayer { get; set; }

        public int Priority { get; }
        public long LastUseTime { get; private set; } = 0;

        public int UserCount { get; set; } = 0;

        public int Position { get; }

        private ASkill skillLogic;

        public int Rate { get; private set; } = 0;


        public SkillState(APlayer player, SkillPanel skillPanel, int position, int useRound)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skillPanel;
            this.Priority = position; // - skillPanel.SkillData.SkillConfig.Priority;
            this.Position = position;
            this.LastUseTime = useRound;

            bool isShow = true;
            if (player.Camp == PlayerType.Enemy && !GameProcessor.Inst.User.ShowMonsterSkill)
            {
                isShow = false;
            }

            if (skillPanel.SkillId == 1010)
            {
                this.skillLogic = new Skill_Chediding(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillId == 2010)
            {
                this.skillLogic = new Skill_Duplication(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillId == 1008)
            {
                this.skillLogic = new Skill_Huti(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillId == 2008)
            {
                this.skillLogic = new Skill_Move(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillId == 3008)
            {
                this.skillLogic = new Skill_Yinshen(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Attack)
            {
                if (skillPanel.SkillData.SkillConfig.CastType == ((int)AttackCastType.Single))
                {
                    this.skillLogic = new Skill_Attack_Single(player, skillPanel, isShow);
                }
                else
                {
                    this.skillLogic = new Skill_Attack_Area(player, skillPanel, isShow);
                }
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Valet)
            {
                this.skillLogic = new Skill_Valet(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Map)
            {
                this.skillLogic = new Skill_Attack_Map(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Restore)
            {
                this.skillLogic = new Skill_Restore(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Shield)
            {
                this.skillLogic = new Skill_Shield(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Expert)
            {
                this.skillLogic = new Skill_Expert(player, skillPanel, isShow);
            }
            else if (skillPanel.SkillData.SkillConfig.Type == (int)SkillType.Yeman)
            {
                this.skillLogic = new Skill_Yeman(player, skillPanel, isShow);
            }
            else
            {
                this.skillLogic = new Skill_Attack_Normal(player, skillPanel, isShow);
            }
        }

        public bool IsCanUse(long Now)
        {
            return (this.LastUseTime == 0 || Now - this.LastUseTime >= this.SkillPanel.CD) && this.skillLogic.IsCanUse();
        }

        public void Do()
        {
            this.LastUseTime = TimeHelper.ClientNowSeconds();
            this.skillLogic.Do();
        }

        public void Do(double baseHp)
        {
            this.LastUseTime = TimeHelper.ClientNowSeconds();
            this.skillLogic.Do(baseHp);
        }

        public void SetLastUseTime(long time)
        {
            this.LastUseTime = time;
        }

        public void AddRate(int rate)
        {
            this.Rate += rate;
        }
    }
}
