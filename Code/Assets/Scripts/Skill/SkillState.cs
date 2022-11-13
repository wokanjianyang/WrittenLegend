using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillState
    {
        public SkillData Data { get; set; }
        public APlayer SelfPlayer { get; set; }

        private ASkill skillLogic;
        
        private int lastUseRound = 0;

        public SkillState(APlayer player, SkillData data)
        {
            this.SelfPlayer = player;
            this.Data = data;
            if (data.ID == 10001)
            {
                this.skillLogic = new Skill_Sweep();
            }
            else
            {
                this.skillLogic = new BaseAttackSkill();
            }
            this.skillLogic.SetParent(player);
        }

        public bool IsCanUse()
        {
            return this.lastUseRound == 0 || this.SelfPlayer.RoundCounter - lastUseRound > this.Data.CD;
        }

        public void Do(int tid)
        {
            this.lastUseRound = this.SelfPlayer.RoundCounter;
            
            this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent()
            {
                TargetId = tid,
                Content = this.Data.Name
            });
            
            this.skillLogic.Do(tid);
        }
    }
}
