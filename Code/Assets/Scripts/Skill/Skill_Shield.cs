using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Shield : ASkill
    {
        public Skill_Shield(APlayer player, SkillPanel skill) : base(player, skill)
        {
            this.skillGraphic = new SkillGraphic_Shield(player, skill);
        }

        public override bool IsCanUse()
        {
            return GetAllTargets().Count > 0;
        }

        public override void Do()
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long total = DamageHelper.GetEffectFromTotal(this.SelfPlayer.AttributeBonus, SkillPanel, effect);

                //Debug.Log("Skill" + effect.Config.Id + " _Shield:" + total);

                DoEffect(this.SelfPlayer, this.SelfPlayer, total, effect);
            }
        }

        public List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();
            attackDatas.Add(new AttackData()
            {
                Tid = this.SelfPlayer.ID,
                Ratio = 1
            });
            return attackDatas;
        }

    }
}
