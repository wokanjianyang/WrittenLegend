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

        public override void Do(SkillRunType runType)
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

            double percent = this.SkillPanel.Percent;
            LargeNumber maxHp = this.SelfPlayer.AttributeBonus.GetTotalAttrLarge(AttributeEnum.HP);
            maxHp.Mul(percent / 100.0);

            this.SelfPlayer.SetHP(maxHp);
            this.SelfPlayer.EventCenter.Raise(new SetPlayerHPEvent { });
        }
    }
}
