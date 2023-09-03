using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Yinshen : ASkill
    {
        public Skill_Yinshen(APlayer player, SkillPanel skill) : base(player, skill)
        {
            this.skillGraphic = new SkillGraphic_Hide(player, skill);
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do()
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            ToHide();

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long total = DamageHelper.GetEffectFromTotal(this.SelfPlayer.AttributeBonus, SkillPanel, effect);

                Debug.Log("Skill" + effect.Config.Id + " _Shield:" + total);

                DoEffect(this.SelfPlayer, this.SelfPlayer, total, effect);
            }
        }

        private void ToHide()
        {
            this.SelfPlayer.IsHide = true;
        }
    }
}
