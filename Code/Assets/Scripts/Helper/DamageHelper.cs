using System;
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

            double roleAttr = GetRoleAttack(attcher, role, true);
            roleAttr = roleAttr * (100 + skill.AttrIncrea + attcher.GetAttackAttr(AttributeEnum.AurasAttrIncrea)) / 100;  //职业攻击

            //防御 = 目标防御 * (100-无视防御)/100
            double def = enemy.GetAttackDoubleAttr(AttributeEnum.Def);
            long ignoreDef = Math.Min(skill.IgnoreDef + attcher.GetAttackAttr(AttributeEnum.DefIgnore), 100);
            def = def * (100 - ignoreDef) / 100;

            double defRiseRate = 1 + enemy.GetAttackDoubleAttr(AttributeEnum.DefendRate);

            double defRate = def * ConfigHelper.Def_Rate * defRiseRate / (def * ConfigHelper.Def_Rate * defRiseRate + roleAttr);

            //if (defRiseRate > 1)
            //{
            //    Debug.Log("defRiseRate:" + defRiseRate + " defRate:" + defRate);
            //}
            double attack = roleAttr * (1 - defRate); //攻击 - 防御

            if (attack <= 0)
            {   //因为精度问题，最高16位，所以防御减伤最高16位
                defRate = def * ConfigHelper.Def_Rate * defRiseRate / roleAttr;
                attack = roleAttr / defRate;
            }

            //韧性减伤
            double strong = enemy.GetAttackDoubleAttr(AttributeEnum.Strong);
            if (strong > 0)
            {
                double relic4 = attcher.GetAttackDoubleAttr(AttributeEnum.Relic4);
                double miss = attcher.GetAttackDoubleAttr(AttributeEnum.Miss);
                if (relic4 * miss >= 1)
                {
                    strong = strong / (relic4 * miss);

                    //Debug.Log("relic4:" + relic4 * miss);
                }

                double shatter = attcher.GetAttackDoubleAttr(AttributeEnum.Shatter) / 100;
                if (shatter > 0)
                {
                    strong = strong / (1 + shatter);
                }

                attack = attack / (1 + strong);
            }

            double parry = enemy.GetAttackDoubleAttr(AttributeEnum.Parry);
            if (parry > 0)
            {
                attack = attack / (1 + parry);
            }

            //技能系数
            attack = attack * (skill.Percent + GetRolePercent(attcher, role)) / 100 + skill.Damage + GetRoleDamage(attcher, role);  // *百分比系数 + 固定数值

            //暴击率 = 攻击者暴击率+技能暴击倍率-被攻击者暴击抵抗率
            long CritRate = attcher.GetAttackAttr(AttributeEnum.CritRate) + skill.CritRate - enemy.GetAttackAttr(AttributeEnum.CritRateResist);

            //DefinitelyCrit 是否必定暴击
            bool isCrit = skill.DefinitelyCrit || RandomHelper.RandomRate((int)CritRate);

            if (isCrit)
            {
                //暴击倍率（ 不低于0 ） = 50基础爆伤+技能爆伤 + 攻击者爆伤 - 被攻击者爆伤减免
                long CritDamage = Math.Max(0, 50 + attcher.GetAttackAttr(AttributeEnum.CritDamage) + skill.CritDamage - enemy.GetAttackAttr(AttributeEnum.CritDamageResist));
                attack = attack * (CritDamage + 100) / 100;
            }

            //绝杀-暴击溢出
            double rcr = attcher.GetAttackDoubleAttr(AttributeEnum.RealCritRate);
            if (rcr > 0 && CritRate > 100)
            {
                attack = attack * (rcr * (CritRate - 100) / 100 + 1);
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

            //职业伤害倍率(物伤加成，法伤加成，道伤加成)
            double roleDamageRise = GetRoleDamageAttackRise(attcher, role, true);

            //Debug.Log("roleDamageRise:" + roleDamageRise);

            attack *= (1 + roleDamageRise / 100);

            //减伤倍率
            double mdr = enemy.CalMulDamageResistAttack();
            attack *= (1 - mdr / 100);

            //增伤倍率
            double mdi = attcher.GetAttackDoubleAttr(AttributeEnum.MulDamageIncrea);
            attack *= (1 + mdi / 100);

            //承受者的易伤
            long ExtraDamage = enemy.GetAttackAttr(AttributeEnum.ExtraDamage);
            attack = attack * (100 + ExtraDamage) / 100;

            //最终伤害加成
            attack = attack * (100 + skill.FinalIncrea) / 100;

            //幸运，每点造成10%最终伤害
            long lucky = attcher.GetAttackAttr(AttributeEnum.Lucky);
            attack = attack * (lucky * 10 + 100) / 100;

            double luckyHit = attcher.GetAttackDoubleAttr(AttributeEnum.LuckyHit);
            if (luckyHit > 0)
            {
                attack = attack * (luckyHit * lucky + 100) / 100;
                //Debug.Log("luckyHit:" + luckyHit + "  rise:" + (luckyHit * lucky + 100) / 100);
            }

            double relic3 = attcher.GetAttackDoubleAttr(AttributeEnum.Relic3);
            if (relic3 > 0)
            {
                double accuracy = attcher.GetAttackDoubleAttr(AttributeEnum.Accuracy);
                attack = attack * (accuracy * relic3 + 100) / 100;

                //Debug.Log("relic3:" + relic3 + "  rise:" + (accuracy * relic3 + 100) / 100);
            }

            double relic5 = attcher.GetAttackDoubleAttr(AttributeEnum.Relic5);
            if (relic5 > 0)
            {
                double speed = attcher.GetAttackDoubleAttr(AttributeEnum.Speed);
                //attack = attack * (speed * relic5 + 100) / 100;

                //Debug.Log("attack:" + attack);

                //Debug.Log("relic5:" + relic5 + "  rise:" + (speed * relic5 + 100) / 100);
            }

            double protect = enemy.GetAttackAttr(AttributeEnum.Protect);
            attack = attack * (100 - protect) / 100;

            MsgType type = isCrit ? MsgType.Crit : MsgType.Damage;

            double extendDamage = 0;
            double at = attcher.GetAttackDoubleAttr(AttributeEnum.RealHpDamage);
            if (at > 0)
            {
                double maxHp = attcher.GetAttackDoubleAttr(AttributeEnum.HP);
                extendDamage = maxHp / (1 + parry) * at;
                //Debug.Log("maxHp:" + StringHelper.FormatNumber(maxHp) + " extendDamage:" + StringHelper.FormatNumber(extendDamage));
            }

            if (double.IsInfinity(attack) || attack > ConfigHelper.MaxNumber)
            {
                attack = ConfigHelper.MaxNumber;
            }
            //Debug.Log("attack:" + StringHelper.FormatNumber(attack));

            //强制最少1点伤害
            return new DamageResult(Math.Max(1, attack), extendDamage, type, (RoleType)role, skill.SkillId); //
        }

        public static bool IsMiss(APlayer self, APlayer enemy, double skillAccuracy)
        {
            double accuracy = self.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Accuracy) + skillAccuracy;
            double miss = enemy.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Miss);

            double rate = 100 + accuracy - miss;

            rate = Math.Max(rate, 20); //闪避最高80%

            //Debug.Log("miss rate:" + rate);

            return !RandomHelper.RandomRate((int)rate);
        }

        public static bool IsMiss2(APlayer self, APlayer enemy)
        {
            double miss = enemy.AttributeBonus.GetTotalAttrDouble(AttributeEnum.Miss2);

            double rate = Math.Min(miss, 95); //闪避最高95%

            //Debug.Log("miss2 rate:" + rate);

            return RandomHelper.RandomRate((int)rate);
        }

        public static long GetSkillDamage(AttributeBonus attributeBonus, int role)
        {
            long attack = 100;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack += attributeBonus.GetAttackAttr(AttributeEnum.SkillPhyDamage);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack += attributeBonus.GetAttackAttr(AttributeEnum.SkillMagicDamage);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack += attributeBonus.GetAttackAttr(AttributeEnum.SkillSpiritDamage);
                        break;
                    }
            }

            attack += attributeBonus.GetAttackAttr(AttributeEnum.SkillAllDamage);

            return attack;
        }

        public static double GetRoleDamageAttackRise(AttributeBonus attributeBonus, int role, bool haveBuff)
        {
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        return attributeBonus.GetTotalAttrDouble(AttributeEnum.PhyDamage, haveBuff);
                    }
                case (int)RoleType.Mage:
                    {
                        return attributeBonus.GetTotalAttrDouble(AttributeEnum.MagicDamage, haveBuff);
                    }
                case (int)RoleType.Warlock:
                    {
                        return attributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritDamage, haveBuff);
                    }
            }

            return 1;
        }

        public static double GetRoleAttack(AttributeBonus attributeBonus, int role, bool haveBuff)
        {
            double attack = 0;
            switch (role)
            {
                case (int)RoleType.Warrior:
                    {
                        attack = attributeBonus.GetTotalAttrDouble(AttributeEnum.PhyAtt, haveBuff);
                        break;
                    }
                case (int)RoleType.Mage:
                    {
                        attack = attributeBonus.GetTotalAttrDouble(AttributeEnum.MagicAtt, haveBuff);
                        break;
                    }
                case (int)RoleType.Warlock:
                    {
                        attack = attributeBonus.GetTotalAttrDouble(AttributeEnum.SpiritAtt, haveBuff);
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

        public static double GetEffectFromTotal(AttributeBonus attacker, SkillPanel skillPanel, EffectData effect)
        {
            int srcAttr = effect.Config.SourceAttr;

            //按照某个属性，计算百分比+固定值得来的
            if (srcAttr == -2)
            {
                double total = attacker.GetTotalAttr((AttributeEnum)effect.Config.SourceAttr);

                //Debug.Log("Shield Base Total:" + total);

                int role = skillPanel.SkillData.SkillConfig.Role;

                double percent = effect.Percent;
                if (effect.Config.ExpertRise > 0) //享受其他增强收益
                {
                    //Debug.Log("Shield Skill-Percent:" + skillPanel.Percent);
                    //Debug.Log("Shield Role-Percent:" + GetRolePercent(attacker, role));

                    //技能系数
                    percent += GetRolePercent(attacker, role) * effect.Config.ExpertRise / 100;
                }

                long damage = effect.Damage;
                if (effect.Config.ExpertRise > 0)
                {
                    damage += GetRoleDamage(attacker, role) * effect.Config.ExpertRise / 100;
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
                double total = effect.Percent;
                return total;
            }

            return 0;
        }
    }

    public class DamageResult
    {
        public DamageResult(double damage, double extendDamage, MsgType type, RoleType roleType, int skillId)
        {
            this.Damage = damage;
            this.ExtendDamage = extendDamage;
            this.Type = type;
            this.RoleType = roleType;
            this.SkillId = skillId;
        }

        public DamageResult(int formId, double damage, MsgType type, RoleType roleType)
        {
            this.FromId = formId;
            this.Damage = damage;
            this.Type = type;
            this.RoleType = roleType;
        }

        public MsgType Type { get; set; }

        public RoleType RoleType { get; set; }
        public double Damage { get; set; }
        public double ExtendDamage { get; set; }
        public int FromId { get; set; }

        public int SkillId { get; set; }
    }
}