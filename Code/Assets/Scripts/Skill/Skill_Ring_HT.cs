using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Ring_HT : ASkill
    {
        public Skill_Ring_HT(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
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

            int percent = this.SkillPanel.Percent;

            double maxHp = this.SelfPlayer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP);
            double sp = maxHp * percent / 100.0;

            this.SelfPlayer.AddSP(sp);
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }
    }
}
