using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Yinshen : ASkill
    {
        public Skill_Yinshen(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Hide(player, skill);
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

            ToHide();

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long total = DamageHelper.GetEffectFromTotal(this.SelfPlayer.AttributeBonus, SkillPanel, effect);

                //Debug.Log("Effect " + effect.Config.Id + " _Percetn:" + total);

                if (effect.Config.TargetType == (int)EffectTarget.Valet)
                {
                    var valets = GameProcessor.Inst.PlayerManager.GetValets(this.SelfPlayer);
                    //Debug.Log("valets count:" + valets.Count);
                    foreach (Valet valet in valets)
                    {
                        DoEffect(valet, this.SelfPlayer, total, effect);
                    }
                }
                else
                {
                    DoEffect(this.SelfPlayer, this.SelfPlayer, total, effect);
                }
            }
        }

        private void ToHide()
        {
            this.SelfPlayer.IsHide = true;
        }
    }
}
