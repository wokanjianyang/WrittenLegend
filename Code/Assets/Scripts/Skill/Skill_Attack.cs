using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    abstract public class Skill_Attack : ASkill
    {
        public Skill_Attack(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
        }

        public override bool IsCanUse()
        {
            //判断距离
            if (SelfPlayer.Enemy == null)
            {
                return false;
            }

            Vector3Int sp = SelfPlayer.Cell;
            Vector3Int ep = SelfPlayer.Enemy.Cell;

            if (SkillPanel.Area == AttackGeometryType.FrontRow || SkillPanel.Area == AttackGeometryType.Cross)
            {
                if (sp.x != ep.x && sp.y != ep.y) //判断是否在直线
                {
                    return false;
                }
            }

            int distance = Math.Abs(sp.x - ep.x) + Math.Abs(sp.y - ep.y) + Math.Abs(sp.z - ep.z);
            if (this.SkillPanel.Dis >= distance) //判断距离
            {
                return true;
            }

            return false;

        }

        public override void Do()
        {
            List<Vector3Int> playCells = GetPlayCells();
            Vector3Int scale = Vector3Int.zero;
            if (SkillPanel.Area == AttackGeometryType.Square)
            {
                scale = new Vector3Int(SkillPanel.Column, SkillPanel.Row, 0);
            }
            else if (SkillPanel.Area == AttackGeometryType.FrontRow)
            {
                scale = new Vector3Int(SkillPanel.Dis, 1, 0);
            }

            this.skillGraphic?.PlayAnimation(playCells, scale);

            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                var damage = this.CalcFormula(enemy, attackData.Ratio);
                enemy.OnHit(attackData.Tid, damage);

                foreach (EffectData effect in SkillPanel.EffectIdList.Values)
                {
                    DoEffect(enemy, this.SelfPlayer, damage, effect);
                }
            }
        }

        public long CalcFormula(APlayer enemy, float ratio)
        {
            //计算公式  ((攻击 - 防御) * 百分比系数 + 固定数值) * 暴击?.暴击倍率 * (伤害加成-伤害减免) * (幸运)

            int role = SkillPanel.SkillData.SkillConfig.Role;

            long roleAttr = SelfPlayer.GetRoleAttack(role) * (100 + SkillPanel.AttrIncrea) / 100;  //职业攻击

            //防御 = 目标防御 * (100-无视防御)/100
            long def = enemy.AttributeBonus.GetTotalAttr(AttributeEnum.Def);
            int ignoreDef = Math.Min(SkillPanel.IgnoreDef, 100);
            def = def * (100 - ignoreDef) / 100;

            long attack = roleAttr - def; //攻击 - 防御

            //技能系数
            attack = attack * (SkillPanel.Percent + SelfPlayer.GetRolePercent(role)) / 100 + SkillPanel.Damage + SelfPlayer.GetRoleDamage(role);  // *百分比系数 + 固定数值

            //暴击率 = 攻击者暴击率+技能暴击倍率-被攻击者暴击抵抗率
            long CritRate = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate) + SkillPanel.CritRate - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist);
            if (RandomHelper.RandomCritRate((int)CritRate))
            {
                //暴击倍率（ 不低于0 ） = 50基础爆伤+技能爆伤 + 攻击者爆伤 - 被攻击者爆伤减免
                long CritDamage = Math.Max(0, 50 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage) + SkillPanel.CritDamage - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist));
                attack = attack * (CritDamage + 100) / 100;
            }

            //伤害加成（不低于0） = 100基础伤害+技能伤害加成 + 攻击者伤害加成 — 被攻击者伤害减免 
            long DamageIncrea = Math.Max(0, 100 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea) + SkillPanel.DamageIncrea - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist));
            attack = attack * DamageIncrea / 100;

            //最终伤害加成
            attack = attack * (100 + SkillPanel.FinalIncrea) / 100;

            //幸运，每点造成10%最终伤害
            long lucky = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky);
            attack = attack * (lucky * 10 + 100) / 100;

            //强制最少1点伤害
            return Math.Max(1, attack);
        }

        abstract public List<AttackData> GetAllTargets();
        abstract public List<Vector3Int> GetPlayCells();
    }
}
