using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Ring_FH : ASkill
    {
        public Skill_Ring_FH(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            this.skillGraphic = null;
        }

        public override bool IsCanUse()
        {
            return true;
        }

        public override void Do()
        {
            this.SelfPlayer.EventCenter.Raise(new ShowMsgEvent()
            {
                Type = MsgType.Ring,
                Content = SkillPanel.SkillData.SkillConfig.Name
            });

            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                DoEffect(this.SelfPlayer, this.SelfPlayer, 0, 0, effect);
            }

            int percent = this.SkillPanel.Percent;
            double maxHp = this.SelfPlayer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
            double hp = maxHp * percent / 100.0;

            this.SelfPlayer.SetHP(hp);
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }
    }
}
