using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Ring_Ht : ASkill
    {
        public Skill_Ring_Ht(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            this.skillGraphic = null;
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do()
        {
            int percent = this.SkillPanel.Percent;

            double maxHp = this.SelfPlayer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
            double sp = maxHp * percent / 100.0;

            this.SelfPlayer.SetSp(sp);
        }
    }
}
