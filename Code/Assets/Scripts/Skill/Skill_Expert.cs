using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Expert : BaseAttackSkill
    {

        public Skill_Expert(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
            this.skillGraphic = null;
        }

        public override bool IsCanUse()
        {
            return false;
        }


    }
}
