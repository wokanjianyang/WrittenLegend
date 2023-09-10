﻿using System;
using System.Text;
using UnityEngine;

namespace Game
{
    public class DamageHelper
    {
        public static DamageResult CalcDamage(AttributeBonus attcher, AttributeBonus enemy, SkillPanel skill)
        {
            //计算公式  ((攻击 - 防御) * 百分比系数 + 固定数值) * 暴击?.暴击倍率 * (伤害加成-伤害减免) * (幸运)

            int role = skill.SkillData.SkillConfig.Role;

            long roleAttr = GetRoleAttack(attcher, role) * (100 + skill.AttrIncrea) / 100;  //职业攻击

            //防御 = 目标防御 * (100-无视防御)/100
            long def = enemy.GetAttackAttr(AttributeEnum.Def);
            int ignoreDef = Math.Min(skill.IgnoreDef, 100);
            def = def * (100 - ignoreDef) / 100;

            double defRate = def * ConfigHelper.Def_Rate / (def * ConfigHelper.Def_Rate + roleAttr);

            long attack = (long)(roleAttr * (1 - defRate)); //攻击 - 防御

            //技能系数
            attack = attack * (skill.Percent + GetRolePercent(attcher, role)) / 100 + skill.Damage + GetRoleDamage(attcher, role);  // *百分比系数 + 固定数值

            //暴击率 = 攻击者暴击率+技能暴击倍率-被攻击者暴击抵抗率
            long CritRate = attcher.GetAttackAttr(AttributeEnum.CritRate) + skill.CritRate - enemy.GetAttackAttr(AttributeEnum.CritRateResist);

            bool isCrit = RandomHelper.RandomCritRate((int)CritRate);
            if (isCrit)
            {
                //暴击倍率（ 不低于0 ） = 50基础爆伤+技能爆伤 + 攻击者爆伤 - 被攻击者爆伤减免
                long CritDamage = Math.Max(0, 50 + attcher.GetAttackAttr(AttributeEnum.CritDamage) + skill.CritDamage - enemy.GetAttackAttr(AttributeEnum.CritDamageResist));
                attack = attack * (CritDamage + 100) / 100;
            }

            //伤害加成（不低于5） = 100基础伤害+技能伤害加成 + 攻击者伤害加成 — 被攻击者伤害减免 
            long DamageIncrea = Math.Max(5, 100 + attcher.GetAttackAttr(AttributeEnum.DamageIncrea) + skill.DamageIncrea - enemy.GetAttackAttr(AttributeEnum.DamageResist));
            attack = attack * DamageIncrea / 100;

            //光环伤害加成（不低于5） = 100基础伤害+技能伤害加成 + 攻击者伤害加成 — 被攻击者伤害减免 
            long AurasDamageIncrea = Math.Max(5, 100 + attcher.GetAttackAttr(AttributeEnum.AurasDamageIncrea) - enemy.GetAttackAttr(AttributeEnum.AurasDamageResist));
            attack = attack * AurasDamageIncrea / 100;

            //技能伤害加成
            long SkillDamage = GetSkillDamage(attcher, role);

            attack = attack * SkillDamage / 100;

            //最终伤害加成
            attack = attack * (100 + skill.FinalIncrea) / 100;

            //幸运，每点造成10%最终伤害
            long lucky = attcher.GetAttackAttr(AttributeEnum.Lucky);
            attack = attack * (lucky * 10 + 100) / 100;

            MsgType type = isCrit ? MsgType.Crit : MsgType.Damage;

            //强制最少1点伤害
            return new DamageResult(Math.Max(1, attack), type); //
        }

        public static long GetSkillDamage(AttributeBonus attributeBonus, int role)
        {
            long attack = 100;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack += attributeBonus.GetAttackAttr(AttributeEnum.PhyDamage);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack += attributeBonus.GetAttackAttr(AttributeEnum.MagicDamage);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack += attributeBonus.GetAttackAttr(AttributeEnum.SpiritDamage);
                        break;
                    }
            }

            attack += attributeBonus.GetAttackAttr(AttributeEnum.AllDamage);

            return attack;
        }

        public static long GetRoleAttack(AttributeBonus attributeBonus, int role)
        {
            long attack = 0;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.PhyAtt);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.MagicAtt);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.SpiritAtt);
                        break;
                    }
            }

            return attack;
        }

        public static long GetRolePercent(AttributeBonus attributeBonus, int role)
        {
            long attack = 0;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.WarriorSkillPercent);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.MageSkillPercent);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.WarlockSkillPercent);
                        break;
                    }
            }

            return attack;
        }

        public static long GetRoleDamage(AttributeBonus attributeBonus, int role)
        {
            long attack = 0;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.WarriorSkillDamage);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.MageSkillDamage);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = attributeBonus.GetAttackAttr(AttributeEnum.WarlockSkillDamage);
                        break;
                    }
            }

            return attack;
        }

        internal static int CalcAttackRound(AttributeBonus attacker, AttributeBonus enemy, SkillPanel offlineSkill)
        {
            var dr = CalcDamage(attacker, enemy, offlineSkill);

            long hp = enemy.GetAttackAttr(AttributeEnum.HP);

            int rd = dr.Damage > 0 ? Math.Min((int)(hp / dr.Damage), 9999999) : 0;

            return Math.Max(rd, 1);
        }

        public static long GetEffectFromTotal(AttributeBonus attacker, SkillPanel skillPanel, EffectData effect)
        {
            int srcAttr = effect.Config.SourceAttr;

            //按照某个属性，计算百分比+固定值得来的
            if (srcAttr > 0)
            {
                long total = attacker.GetTotalAttr((AttributeEnum)effect.Config.SourceAttr);

                //Debug.Log("Shield Base Total:" + total);

                int role = skillPanel.SkillData.SkillConfig.Role;

                long percent = 0;
                if (effect.Config.PercentGain > 0) //享受其他增强收益
                {
                    //Debug.Log("Shield Skill-Percent:" + skillPanel.Percent);
                    //Debug.Log("Shield Role-Percent:" + GetRolePercent(attacker, role));

                    //技能系数
                    percent = (skillPanel.Percent + GetRolePercent(attacker, role)) * effect.Config.PercentGain / 100;
                }

                long damage = 0;
                if (effect.Config.ConstGain > 0)
                {
                    damage = (skillPanel.Damage + GetRoleDamage(attacker, role)) * effect.Config.ConstGain / 100;

                    //Debug.Log("Shield Damage:" + damage);
                }

                //Debug.Log("Shield Percent:" + percent);

                total = total * percent / 100 + damage;   // *百分比系数 + 固定数值

                //Debug.Log("Shield Gain Total :" + total);

                return total;
            }
            //配置来源的数值
            else if (srcAttr == 0)
            {
                long total = effect.Percent;
                return total;
            }

            return 0;
        }
    }

    public class DamageResult
    {
        public DamageResult(long damage, MsgType type) {
            this.Damage = damage;
            this.Type = type;
        }

        public DamageResult(int formId,long damage, MsgType type)
        {
            this.FromId = formId;
            this.Damage = damage;
            this.Type = type;
        }

        public MsgType Type { get; set; }
        public long Damage { get; set; }
        public int FromId { get; set; }
    }
}