using System;
using System.Text;

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
            long def = enemy.GetTotalAttr(AttributeEnum.Def);
            int ignoreDef = Math.Min(skill.IgnoreDef, 100);
            def = def * (100 - ignoreDef) / 100;

            double defRate = def * ConfigHelper.Def_Rate / (def * ConfigHelper.Def_Rate + roleAttr);

            long attack = (long)(roleAttr * (1 - defRate)); //攻击 - 防御

            //技能系数
            attack = attack * (skill.Percent + GetRolePercent(attcher, role)) / 100 + skill.Damage + GetRoleDamage(attcher, role);  // *百分比系数 + 固定数值

            //暴击率 = 攻击者暴击率+技能暴击倍率-被攻击者暴击抵抗率
            long CritRate = attcher.GetTotalAttr(AttributeEnum.CritRate) + skill.CritRate - enemy.GetTotalAttr(AttributeEnum.CritRateResist);

            bool isCrit = RandomHelper.RandomCritRate((int)CritRate);
            if (isCrit)
            {
                //暴击倍率（ 不低于0 ） = 50基础爆伤+技能爆伤 + 攻击者爆伤 - 被攻击者爆伤减免
                long CritDamage = Math.Max(0, 50 + attcher.GetTotalAttr(AttributeEnum.CritDamage) + skill.CritDamage - enemy.GetTotalAttr(AttributeEnum.CritDamageResist));
                attack = attack * (CritDamage + 100) / 100;
            }

            //伤害加成（不低于5） = 100基础伤害+技能伤害加成 + 攻击者伤害加成 — 被攻击者伤害减免 
            long DamageIncrea = Math.Max(5, 100 + attcher.GetTotalAttr(AttributeEnum.DamageIncrea) + skill.DamageIncrea - enemy.GetTotalAttr(AttributeEnum.DamageResist));
            attack = attack * DamageIncrea / 100;

            //最终伤害加成
            attack = attack * (100 + skill.FinalIncrea) / 100;

            //幸运，每点造成10%最终伤害
            long lucky = attcher.GetTotalAttr(AttributeEnum.Lucky);
            attack = attack * (lucky * 10 + 100) / 100;

            MsgType type = isCrit ? MsgType.Crit : MsgType.Damage;

            //强制最少1点伤害
            return new DamageResult(Math.Max(1, attack), type); //
        }

        public static long GetRoleAttack(AttributeBonus attributeBonus,int role)
        {
            long attack = 0;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.PhyAtt);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.MagicAtt);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.SpiritAtt);
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
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.WarriorSkillPercent);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.MageSkillPercent);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.WarlockSkillPercent);
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
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.WarriorSkillDamage);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.MageSkillDamage);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = attributeBonus.GetTotalAttr(AttributeEnum.WarlockSkillDamage);
                        break;
                    }
            }

            return attack;
        }

        internal static int CalcAttackRound(AttributeBonus attacker, AttributeBonus enemy, SkillPanel offlineSkill)
        {
            var dr = CalcDamage(attacker, enemy, offlineSkill);

            long hp = enemy.GetTotalAttr(AttributeEnum.HP);

            return dr.Damage > 0 ? Math.Min((int)(hp / dr.Damage), 9999999) : 0;
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