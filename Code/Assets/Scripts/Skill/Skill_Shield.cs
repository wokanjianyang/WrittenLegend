using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Shield : ASkill
    {
        public Skill_Shield(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
            this.skillGraphic = null;
        }

        public override bool IsCanUse()
        {
            return GetAllTargets().Count > 0;
        }

        public override void Do()
        {
            //如果还有附加特效
            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);
            //
            SelfPlayer.EventCenter.Raise(new ShowMsgEvent
            {
                Type = MsgType.SkillName,
                Content = SkillPanel.SkillData.SkillConfig.Name
            });

            //对自己加属性Buff
            foreach (EffectData effect in SkillPanel.EffectIdList.Values)
            {
                long total = CalcFormula();

                DoEffect(this.SelfPlayer, this.SelfPlayer, total, effect);
            }
        }

        public long CalcFormula()
        {
            //Buff不计暴击增伤幸运等
            int role = SkillPanel.SkillData.SkillConfig.Role;

            long roleAttr = SelfPlayer.GetRoleAttack(role) * (100 + SkillPanel.AttrIncrea) / 100;  //职业攻击

            //技能系数
            long attack = roleAttr * (SkillPanel.Percent + SelfPlayer.GetRolePercent(role)) / 100 + SkillPanel.Damage + SelfPlayer.GetRoleDamage(role);  // *百分比系数 + 固定数值

            return attack;
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
