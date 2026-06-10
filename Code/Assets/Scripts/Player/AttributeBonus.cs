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

        private Dictionary<AttributeEnum, Dictionary<int, LargeNumber>> LargeDict = new Dictionary<AttributeEnum, Dictionary<int, LargeNumber>>();

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

        public void SetAttrLarge(AttributeEnum attrType, AttributeFrom attrKey, LargeNumber attrValue)
        {
            this.SetAttrLarge(attrType, (int)attrKey, attrValue);
        }

        public void SetAttrLarge(AttributeEnum attrType, int attrKey, LargeNumber attrValue)
        {
            if (!LargeDict.ContainsKey(attrType))
            {
                LargeDict.Add(attrType, new Dictionary<int, LargeNumber>());
            }

            LargeDict[attrType][attrKey] = attrValue;
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

        public LargeNumber GetTotalAttrLarge(AttributeEnum attrType)
        {
            double total = GetTotalAttrDouble(attrType);
            LargeNumber lg = new LargeNumber(total);

            if (LargeDict.ContainsKey(attrType))
            {
                if ((int)attrType < -999999)
                {
                    foreach (LargeNumber val in LargeDict[attrType].Values)
                    {
                        lg.Add(val);
                    }
                }
                else
                {
                    foreach (LargeNumber val in LargeDict[attrType].Values)
                    {
                        lg.Mul(val);
                    }
                }
            }

            return lg;
        }


        public LargeNumber GetTotalAtkLarge(int role)
        {
            LargeNumber lg = GetSingleAttrLarge(AttributeEnum.MulAttr);

            LargeNumber lg1;

            if (role == 1)
            {
                lg1 = GetSingleAttrLarge(AttributeEnum.MulAttrPhy);
            }
            else if (role == 2)
            {
                lg1 = GetSingleAttrLarge(AttributeEnum.MulAttrMagic);
            }
            else
            {
                lg1 = GetSingleAttrLarge(AttributeEnum.MulAttrSpirit);
            }

            if (lg1.data > 0)
            {
                lg.Mul(lg1);
            }

            return lg;

        }

        public LargeNumber GetSingleAttrLarge(AttributeEnum attrType)
        {
            LargeNumber lg = new LargeNumber(1);

            if (LargeDict.ContainsKey(attrType))
            {
                if ((int)attrType < -999999)
                {
                    foreach (LargeNumber val in LargeDict[attrType].Values)
                    {
                        lg.Add(val);
                    }
                }
                else
                {
                    foreach (LargeNumber val in LargeDict[attrType].Values)
                    {
                        lg.Mul(val);
                    }
                }
            }

            return lg;
        }

        public LargeNumber CalUserMulTotalLarge(AttributeEnum percentType)
        {
            LargeNumber lg = new LargeNumber(1);

            if (AllAttrDict.ContainsKey(percentType))
            {
                foreach (double pc in AllAttrDict[percentType].Values)
                {
                    lg.Mul((100.0 + pc) / 100.0);
                }
            }

            return lg;
        }

        public double GetUserTotalAttrDouble(AttributeEnum attrType)
        {
            double total = 0;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalTotal(AttributeEnum.HP, false, AttributeEnum.HpIncrea) * (CalTotal(AttributeEnum.PanelHp, false) + 100) / 100;
                    break;
                case AttributeEnum.PhyAtt:
                    total = CalTotal(AttributeEnum.PhyAtt, false, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) * (CalTotal(AttributeEnum.PanelPhyAtt, false) + 100) / 100;
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalTotal(AttributeEnum.MagicAtt, false, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) * (CalTotal(AttributeEnum.PanelMagicAtt, false) + 100) / 100;
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalTotal(AttributeEnum.SpiritAtt, false, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) * (CalTotal(AttributeEnum.PanelSpiritAtt, false) + 100) / 100;
                    break;
                case AttributeEnum.Strong:
                    total = CalTotal(AttributeEnum.Strong, false);
                    break;
                case AttributeEnum.PhyDamage:
                    total = 100 + CalTotal(AttributeEnum.PhyDamage, false);
                    break;
                case AttributeEnum.MagicDamage:
                    total = 100 + CalTotal(AttributeEnum.MagicDamage, false);
                    break;
                case AttributeEnum.SpiritDamage:
                    total = 100 + CalTotal(AttributeEnum.SpiritDamage, false);
                    break;
                default:
                    throw new Exception("not implete type " + attrType.ToString());
                    break;
            }

            return total;
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

        public LargeNumber GetBaseAttrLarge(AttributeEnum attrType)
        {
            double total = GetBaseAttr(attrType);

            LargeNumber lg = new LargeNumber(total);

            if (LargeDict.ContainsKey(attrType))
            {
                if ((int)attrType < -999999)
                {
                    foreach (LargeNumber val in LargeDict[attrType].Values)
                    {
                        lg.Add(val);
                    }
                }
                else
                {
                    foreach (LargeNumber val in LargeDict[attrType].Values)
                    {
                        lg.Mul(val);
                    }
                }
            }

            return lg;
        }


        public LargeNumber GetPowerNew()
        {
            double p1 = GetUserTotalAttrDouble(AttributeEnum.PhyAtt);
            double p2 = GetUserTotalAttrDouble(AttributeEnum.MagicAtt);
            double p3 = GetUserTotalAttrDouble(AttributeEnum.SpiritAtt);

            int role = 1;
            double powerDamage = p1;

            LargeNumber mulRoleAtk = CalUserMulTotalLarge(AttributeEnum.MulAttrPhy);

            if (p2 > powerDamage)
            {
                role = 2;
                powerDamage = p2;

                mulRoleAtk = CalUserMulTotalLarge(AttributeEnum.MulAttrMagic);
            }
            if (p3 > powerDamage)
            {
                role = 3;
                powerDamage = p3;

                mulRoleAtk = CalUserMulTotalLarge(AttributeEnum.MulAttrSpirit);
            }

            LargeNumber lg = new LargeNumber(powerDamage);

            LargeNumber mulAtk = CalUserMulTotalLarge(AttributeEnum.MulAttr);
            if (mulAtk.data > 0)
            {
                lg.Mul(mulAtk);
            }

            if (mulRoleAtk.data > 0)
            {
                lg.Mul(mulRoleAtk);
            }

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

            LargeNumber lg1 = new LargeNumber(powerDef);

            lg1.Mul(1 + CalPercent(AttributeEnum.DamageResist) * CalPercent(AttributeEnum.AurasDamageResist));
            lg1.Mul(1 + CalPercent(AttributeEnum.Miss));
            lg1.Mul(1 + GetTotalAttrDouble(AttributeEnum.Strong));

            //减伤倍率
            double mdr = CalMulDamageResist(false);
            lg1.Mul(1 / (1 - mdr / 100));

            lg.Add(lg1);
            lg.Div(20);

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