using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Duplication : ASkill
    {
        public Skill_Duplication(APlayer player, SkillPanel skillPanel, bool isShow) : base(player, skillPanel)
        {
            this.skillGraphic = null;
        }
        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do()
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            APlayer duplication = new PlayerDuplication(SelfPlayer, this.SkillPanel);

            GameProcessor.Inst.PlayerManager.LoadDuplication(SelfPlayer, duplication);
        }
    }
}
