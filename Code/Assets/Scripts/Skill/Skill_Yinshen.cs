using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Yinshen : ASkill
    {
        public Skill_Yinshen(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            this.skillGraphic = new SkillGraphic_Hide(player, skill);
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do()
        {
            ToHide();
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long rolePercent = DamageHelper.GetRolePercent(this.SelfPlayer.AttributeBonus, SkillPanel.SkillData.SkillConfig.Role);

                //Debug.Log("Effect " + effect.Config.Id + " _Percetn:" + total);

                if (effect.Config.TargetType == (int)EffectTarget.Valet)
                {
                    var valets = GameProcessor.Inst.PlayerManager.GetValets(this.SelfPlayer);
                    //Debug.Log("valets count:" + valets.Count);
                    foreach (Valet valet in valets)
                    {
                        DoEffect(valet, this.SelfPlayer, 0, rolePercent, effect);
                    }
                }
                else
                {
                    DoEffect(this.SelfPlayer, this.SelfPlayer, 0, rolePercent, effect);
                }
            }
        }

        private void ToHide()
        {
            this.SelfPlayer.IsHide = true;
        }
    }
}
