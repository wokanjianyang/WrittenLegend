using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Ring_FQ : ASkill
    {
        public Skill_Ring_FQ(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
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
        }
    }
}
