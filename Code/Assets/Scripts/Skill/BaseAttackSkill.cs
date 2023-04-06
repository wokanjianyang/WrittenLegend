using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BaseAttackSkill : ASkill
    {
        public BaseAttackSkill(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
        }


        public override float CalcFormula(APlayer player, float ratio)
        {
            return this.SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt) * ratio;
        }

    }
}
