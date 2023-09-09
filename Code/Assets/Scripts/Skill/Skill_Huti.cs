using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Huti : ASkill
    {
        public Skill_Huti(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Shield(player, skill);
            }
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do()
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long total = DamageHelper.GetEffectFromTotal(this.SelfPlayer.AttributeBonus, SkillPanel, effect);

                //Debug.Log("Effect " + effect.Config.Id + " _Percetn:" + total);

                DoEffect(this.SelfPlayer, this.SelfPlayer, total, effect);
            }
        }
    }
}
