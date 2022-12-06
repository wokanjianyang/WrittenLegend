using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BaseAttackSkill : ASkill
    {
        public BaseAttackSkill(APlayer player, SkillData skillData) : base(player, skillData)
        {
        }


        public override float CalcFormula(APlayer player, float ratio)
        {
            return this.SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt) * ratio;
        }

    }
}
