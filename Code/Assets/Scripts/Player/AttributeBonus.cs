using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Game
{
    public class AttributeBonus
    {
        private Dictionary<AttributeEnum, Dictionary<int, double>> AllAttrDict = new Dictionary<AttributeEnum, Dictionary<int, double>>();

        private Dictionary<AttributeEnum, List<DefendBuffConfig>> BuffDict = new Dictionary<AttributeEnum, List<DefendBuffConfig>>();

        private Dictionary<AttributeEnum, Dictionary<int, double>> SkillDict = new Dictionary<AttributeEnum, Dictionary<int, double>>();

        public AttributeBonus()
        {
            foreach (AttributeEnum item in Enum.GetValues(typeof(AttributeEnum)))
            {
                AllAttrDict.Add(item, new Dictionary<int, double>());
                SkillDict.Add(item, new Dictionary<int, double>());
            }
        }

        public void SetBuffList(List<DefendBuffConfig> list)
        {
            this.BuffDict.Clear();

            foreach (DefendBuffConfig config in list)
            {
                AttributeEnum key = (AttributeEnum)config.AttrId;

                BuffDict.TryGetValue(key, out List<DefendBuffConfig> attrList);
                if (attrList == null)
                {
                    attrList = new List<DefendBuffConfig>();
                    BuffDict[key] = attrList;
                }

                attrList.Add(config);
            }
        }

        public void SetSkillAttr(AttributeEnum attrType, int attrKey, double attrValue)
        {
            int key = (int)attrKey;
            SkillDict[attrType][key] = attrValue;
        }

        public void SetAttr(AttributeEnum attrType, AttributeFrom attrKey, double attrValue)
        {
            int key = (int)attrKey;
            AllAttrDict[attrType][key] = attrValue;
        }

        public void SetAttr(AttributeEnum attrType, int attrKey, double attrValue)
        {
            AllAttrDict[attrType][attrKey] = attrValue;
        }


        public void SetAttr(AttributeEnum attrType, AttributeFrom attrKey, int Position, double attrValue)
        {
            int key = ((int)attrKey) * 99999 + Position;
            AllAttrDict[attrType][key] = attrValue;
        }

        public long GetTotalAttr(AttributeEnum attrType)
        {
            return (long)GetTotalAttrDouble(attrType);
        }

        public long GetAttackAttr(AttributeEnum attrType)
        {
            return (long)GetTotalAttrDouble(attrType);
        }

        public double GetAttackDoubleAttr(AttributeEnum attrType)
        {
            return GetTotalAttrDouble(attrType);
        }

        public double GetTotalAttrDouble(AttributeEnum attrType)
        {
            return GetTotalAttrDouble(attrType, true);
        }

        public double GetTotalAttrDouble(AttributeEnum attrType, bool haveBuff)
        {
            double total = 0;
            double mr = 1;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalTotal(AttributeEnum.HP, haveBuff, AttributeEnum.HpIncrea) * (CalTotal(AttributeEnum.PanelHp, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulHp) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.PhyAtt:
                    total = CalTotal(AttributeEnum.PhyAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) * (CalTotal(AttributeEnum.PanelPhyAtt, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrPhy) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalTotal(AttributeEnum.MagicAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) * (CalTotal(AttributeEnum.PanelMagicAtt, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrMagic) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalTotal(AttributeEnum.SpiritAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) * (CalTotal(AttributeEnum.PanelSpiritAtt, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrSpirit) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.Def:
                    total = CalTotal(AttributeEnum.Def, haveBuff, AttributeEnum.DefIncrea) * (CalTotal(AttributeEnum.PanelDef, haveBuff) + 100) / 100;
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.MulDef) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.Strong:
                    total = CalTotal(AttributeEnum.Strong, haveBuff);
                    mr = 1 + CalMulTotal(haveBuff, AttributeEnum.StrongMul) / 100;
                    total *= mr;
                    break;
                case AttributeEnum.PhyDamage:
                    total = 100 + CalTotal(AttributeEnum.PhyDamage, haveBuff);
                    total = total * (1 + CalMulTotal(haveBuff, AttributeEnum.MulPhyDamageRise) / 100) - 100;
                    break;
                case AttributeEnum.CritRate:
                    total = CalTotal(attrType, haveBuff, AttributeEnum.CritFinal);
                    break;
                case AttributeEnum.Lucky:
                    total = CalTotal(attrType, haveBuff, AttributeEnum.LuckyFinal);
                    break;
                case AttributeEnum.MagicDamage:
                    total = 100 + CalTotal(AttributeEnum.MagicDamage, haveBuff);
                    total = total * (1 + CalMulTotal(haveBuff, AttributeEnum.MulMagicDamageRise) / 100) - 100;
                    break;
                case AttributeEnum.SpiritDamage:
                    total = 100 + CalTotal(AttributeEnum.SpiritDamage, haveBuff);
                    total = total * (1 + CalMulTotal(haveBuff, AttributeEnum.MulSpiritDamageRise) / 100) - 100;
                    break;
                case AttributeEnum.MulDamageResist:
                    total = CalMulDamageResist(haveBuff);
                    break;
                case AttributeEnum.SecondExp:
                    total = CalTotal(AttributeEnum.SecondExp, haveBuff, AttributeEnum.ExpIncrea);
                    break;
                case AttributeEnum.SecondGold:
                    total = CalTotal(AttributeEnum.SecondGold, haveBuff, AttributeEnum.GoldIncrea);
                    break;
                case AttributeEnum.ExpIncrea:
                    total = CalTotal(AttributeEnum.ExpIncrea, haveBuff, AttributeEnum.ExpFinal);
                    break;
                case AttributeEnum.GoldIncrea:
                    total = CalTotal(AttributeEnum.GoldIncrea, haveBuff, AttributeEnum.GoldFinal);
                    break;
                case AttributeEnum.BurstIncrea:
                    total = CalTotal(AttributeEnum.BurstIncrea, haveBuff, AttributeEnum.BurstFinal);
                    break;
                case AttributeEnum.QualityIncrea:
                    total = CalTotal(AttributeEnum.QualityIncrea, haveBuff, AttributeEnum.QualityFinal);
                    break;
                case AttributeEnum.MythAttr:
                    total = CalTotal(AttributeEnum.MythAttr, haveBuff) + CalTotal(AttributeEnum.MythAll, haveBuff);
                    break;
                case AttributeEnum.MythDef:
                    total = CalTotal(AttributeEnum.MythDef, haveBuff) + CalTotal(AttributeEnum.MythAll, haveBuff);
                    break;
                case AttributeEnum.MythHp:
                    total = CalTotal(AttributeEnum.MythHp, haveBuff) + CalTotal(AttributeEnum.MythAll, haveBuff);
                    break;
                default:
                    if ((int)attrType < 2001)
                    {
                        total = CalTotal(attrType, haveBuff);
                    }
                    else
                    {
                        total = CalMulTotal(haveBuff, attrType);
                    }
                    break;
            }

            return total;
        }

        public double GetBaseAttr(AttributeEnum attrType)
        {
            if ((int)attrType < 2001)
            {
                return CalTotal(attrType, false);
            }
            else if (attrType == AttributeEnum.MulDamageResist)
            {
                return CalMulDamageResist(false);
            }
            else
            {
                return CalMulTotal(false, attrType);
            }
        }


        public double GetPower()
        {
            double p1 = GetTotalAttrDouble(AttributeEnum.PhyAtt);
            double p2 = GetTotalAttrDouble(AttributeEnum.MagicAtt);
            double p3 = GetTotalAttrDouble(AttributeEnum.SpiritAtt);

            int role = 1;
            double powerDamage = p1;

            if (p2 > powerDamage)
            {
                role = 2;
                powerDamage = p2;
            }
            if (p3 > powerDamage)
            {
                role = 3;
                powerDamage = p3;
            }

            powerDamage *= CalPercent(AttributeEnum.AurasAttrIncrea);
            powerDamage *= CalPercent(AttributeEnum.DamageIncrea) * CalPercent(AttributeEnum.AurasDamageIncrea);
            powerDamage *= (1 + GetTotalAttrDouble(AttributeEnum.Lucky) * 0.1);
            powerDamage *= (1 + Math.Min(GetTotalAttrDouble(AttributeEnum.CritRate), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamage) + 150) / 100);

            double roleDamageRise = DamageHelper.GetRoleDamageAttackRise(this, role, true);
            powerDamage *= (1 + roleDamageRise / 100);

            //增伤倍率
            double mdi = GetTotalAttrDouble(AttributeEnum.MulDamageIncrea);
            powerDamage *= (1 + mdi / 100);
            //破刃倍率
            double sdi = GetTotalAttrDouble(AttributeEnum.Shatter);
            powerDamage *= (1 + sdi);


            double powerDef = GetTotalAttrDouble(AttributeEnum.HP) / 10 + GetTotalAttrDouble(AttributeEnum.Def) * 3;
            powerDef *= (1 + CalPercent(AttributeEnum.DamageResist) * CalPercent(AttributeEnum.AurasDamageResist));
            powerDamage *= (1 + Math.Min(GetTotalAttrDouble(AttributeEnum.CritRateResist), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamageResist) + 100) / 100);
            powerDef *= (1 + CalPercent(AttributeEnum.Miss));
            powerDef *= (1 + GetTotalAttrDouble(AttributeEnum.Strong));

            //减伤倍率
            double mdr = CalMulDamageResist(false);
            powerDef *= 1 / (1 - mdr / 100);

            double newPower = (powerDamage + powerDef) / 20;
            return newPower;
        }

        public LargeNumber GetPowerNew()
        {
            double p1 = GetTotalAttrDouble(AttributeEnum.PhyAtt);
            double p2 = GetTotalAttrDouble(AttributeEnum.MagicAtt);
            double p3 = GetTotalAttrDouble(AttributeEnum.SpiritAtt);

            int role = 1;
            double powerDamage = p1;

            if (p2 > powerDamage)
            {
                role = 2;
                powerDamage = p2;
            }
            if (p3 > powerDamage)
            {
                role = 3;
                powerDamage = p3;
            }

            LargeNumber lg = new LargeNumber(powerDamage);

            lg.Mul(CalPercent(AttributeEnum.AurasAttrIncrea));
            lg.Mul(CalPercent(AttributeEnum.DamageIncrea) * CalPercent(AttributeEnum.AurasDamageIncrea));
            lg.Mul((1 + GetTotalAttrDouble(AttributeEnum.Lucky) * 0.1));
            lg.Mul((1 + Math.Min(GetTotalAttrDouble(AttributeEnum.CritRate), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamage) + 150) / 100));
    
            double roleDamageRise = DamageHelper.GetRoleDamageAttackRise(this, role, true);
            lg.Mul((1 + roleDamageRise / 100));

            //增伤倍率
            double mdi = GetTotalAttrDouble(AttributeEnum.MulDamageIncrea);
            lg.Mul((1 + mdi / 100));
            //破刃倍率
            double sdi = GetTotalAttrDouble(AttributeEnum.Shatter);
            lg.Mul((1 + sdi));
            lg.Mul((1 + Math.Min(GetTotalAttrDouble(AttributeEnum.CritRateResist), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamageResist) + 100) / 100));

            double powerDef = GetTotalAttrDouble(AttributeEnum.HP) / 10 + GetTotalAttrDouble(AttributeEnum.Def) * 3;
            powerDef *= (1 + CalPercent(AttributeEnum.DamageResist) * CalPercent(AttributeEnum.AurasDamageResist));
            powerDef *= (1 + CalPercent(AttributeEnum.Miss));
            powerDef *= (1 + GetTotalAttrDouble(AttributeEnum.Strong));

            //减伤倍率
            double mdr = CalMulDamageResist(false);
            powerDef *= 1 / (1 - mdr / 100);

            lg.add(powerDef);
            lg.div(20);

            return lg;
        }

        public string GetPowerText()
        {
            //return StringHelper.FormatNumber(GetPower());

            return GetPowerNew().FormatUnit();
        }

        private double CalPercent(AttributeEnum type)
        {
            return (100 + GetTotalAttrDouble(type)) / 100;
        }

        private double CalTotal(AttributeEnum type, bool haveBuff, params AttributeEnum[] increaTypes)
        {
            double total = 0;

            foreach (double hp in AllAttrDict[type].Values)
            {
                total += hp;
            }

            if (haveBuff && BuffDict.ContainsKey(type))
            {
                foreach (var item in BuffDict[type])
                {
                    total += item.AttrValue;
                }

            }
            if (haveBuff && SkillDict.ContainsKey(type))
            {
                foreach (var item in SkillDict[type])
                {
                    total += item.Value;
                }
            }

            double percent = 0;

            for (int i = 0; i < increaTypes.Length; i++)
            {
                AttributeEnum percentType = increaTypes[i];
                foreach (double pc in AllAttrDict[percentType].Values)
                {
                    percent += pc;
                }

                if (haveBuff && BuffDict.ContainsKey(percentType))
                {
                    foreach (var item in BuffDict[percentType])
                    {
                        percent += item.AttrValue;
                    }

                }
                if (haveBuff && SkillDict.ContainsKey(percentType))
                {
                    foreach (var item in SkillDict[type])
                    {
                        total += item.Value;
                    }
                }
            }
            return total * (100.0 + percent) / 100.0;
        }

        public double CalMulTotal(bool haveBuff, params AttributeEnum[] mulTypes)
        {
            double total = 100;

            for (int i = 0; i < mulTypes.Length; i++)
            {
                AttributeEnum percentType = mulTypes[i];
                foreach (double pc in AllAttrDict[percentType].Values)
                {
                    total *= (100.0 + pc) / 100.0;
                }

                if (haveBuff && BuffDict.ContainsKey(percentType))
                {
                    foreach (var item in BuffDict[percentType])
                    {
                        total *= (100.0 + item.AttrValue) / 100.0;
                    }

                }
                if (haveBuff && SkillDict.ContainsKey(percentType))
                {
                    foreach (var item in SkillDict[percentType])
                    {
                        total *= (100.0 + item.Value) / 100.0;
                    }
                }
            }

            return total - 100;
        }

        public double CalMulDamageResist(bool haveBuff)
        {
            double total = 1;

            AttributeEnum percentType = AttributeEnum.MulDamageResist;

            long rmdr = GetTotalAttr(AttributeEnum.RealMulDamageResist); //是否使用迭代减伤
            foreach (double pc in AllAttrDict[percentType].Values)
            {
                double fp = CalMulDamageResistLimit(pc, rmdr);

                total *= (1 - fp / 100);
            }

            if (haveBuff && BuffDict.ContainsKey(percentType))
            {
                foreach (var item in BuffDict[percentType])
                {
                    double fp = Math.Min(70.0, item.AttrValue);

                    total *= (1 - fp / 100);
                }

            }
            if (haveBuff && SkillDict.ContainsKey(percentType))
            {
                foreach (var item in SkillDict[percentType])
                {
                    double fp = Math.Min(70.0, item.Value);

                    total *= (1 - fp / 100);
                }
            }

            total = (1 - total) * 100.0;

            if (total >= 100)
            {
                total = 99.9999999999999;
            }

            return total;
        }


        public double CalMulDamageResistLimit(double pc, long rmdr)
        {
            //TODO
            if (rmdr > 0)
            {
                return MathHelper.CalRealResist(pc);
            }
            else
            {
                return Math.Min(70.0, pc);
            }
        }

        public double CalMulDamageResistAttack()
        {
            double total = 1;

            AttributeEnum percentType = AttributeEnum.MulDamageResist;

            foreach (double pc in AllAttrDict[percentType].Values)
            {
                total *= (1 - pc / 100);
            }

            return (1 - total) * 100;
        }
    }
}