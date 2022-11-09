using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillState
    {
        public SkillData Data { get; set; }
        public APlayer SelfPlayer { get; set; }
        
        private int lastUseRound = 0;

        public SkillState(APlayer player, SkillData data)
        {
            this.SelfPlayer = player;
            this.Data = data;
        }

        public bool IsCanUse()
        {
            return this.lastUseRound == 0 || this.SelfPlayer.RoundCounter - lastUseRound > this.Data.CD;
        }

        public void Do()
        {
            this.lastUseRound = this.SelfPlayer.RoundCounter;
        }
    }
}
