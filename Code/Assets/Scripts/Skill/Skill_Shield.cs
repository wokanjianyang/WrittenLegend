using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Shield : BaseAttackSkill
    {
        public Skill_Shield(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
            this.skillGraphic = null;
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();
            attackDatas.Add(new AttackData()
            {
                Tid = this.SelfPlayer.ID,
                Ratio = 1
            });
            return attackDatas;
        }

        public override bool IsCanUse()
        {
            return GetAllTargets().Count > 0;
        }

        public override void Do()
        {
            //对自己加属性Buff
            SkillConfig skillConfig = SkillPanel.SkillData.SkillConfig;
            if (skillConfig.EffectIdList != null && skillConfig.EffectIdList.Length > 0)
            {
                long total = CalcFormula();
                foreach (int EffectId in skillConfig.EffectIdList)
                {
                    if (EffectId > 0)
                    {
                        EffectConfig config = EffectConfigCategory.Instance.Get(EffectId);

                        var effectTarget = this.SelfPlayer; //1 为作用自己 2 为作用敌人
                        int fromId = (int)AttributeFrom.Skill * 100000 + SkillPanel.SkillId + EffectId;
                        int duration = SkillPanel.Duration;
                        if (config.Duration > 0)
                        {  //持续Buff
                            effectTarget.AddEffect(EffectId, this.SelfPlayer, fromId, total, duration);
                        }
                        else
                        {
                            effectTarget.RunEffect(EffectId, this.SelfPlayer, fromId, total, duration);
                        }
                    }
                }
            }

            this.skillGraphic?.PlayAnimation(SelfPlayer.Cell);

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
    }
}
