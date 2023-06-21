using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillMapState
    {
        //public APlayer SelfPlayer { get; set; }

        private Skill_Attack_Map skill;

        private int Duration = 0;

        public SkillMapState(Skill_Attack_Map skill)
        {
            //this.SelfPlayer = player;
            this.skill = skill;
            this.Duration = 0;
        }

        public void Run(APlayer enemy)
        {
            this.Duration++;

            if (enemy == null)
            { //空格子
                return;
            }

            skill.Run(enemy);
        }

        public bool IsOver()
        {
            return Duration >= skill.SkillPanel.Duration;
        }
    }
}
