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

        public AttributeBonus()
        {
            foreach (AttributeEnum item in Enum.GetValues(typeof(AttributeEnum)))
            {
                AllAttrDict.Add(item, new Dictionary<int, double>());
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
            int key = ((int)attrKey) * 9999 + Position;
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

        public double GetTotalAttrDouble(AttributeEnum attrType)
        {
            return GetTotalAttrDouble(attrType, true);
        }

        public double GetTotalAttrDouble(AttributeEnum attrType, bool haveBuff)
        {
            double total = 0;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalTotal(AttributeEnum.HP, haveBuff, AttributeEnum.HpIncrea) * (CalTotal(AttributeEnum.PanelHp, haveBuff) + 100) / 100;
                    total = CalMulTotal(total, haveBuff, AttributeEnum.MulHp);
                    break;
                case AttributeEnum.PhyAtt:
                    total = CalTotal(AttributeEnum.PhyAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) * (CalTotal(AttributeEnum.PanelPhyAtt, haveBuff) + 100) / 100;
                    total = CalMulTotal(total, haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrPhy);
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalTotal(AttributeEnum.MagicAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) * (CalTotal(AttributeEnum.PanelMagicAtt, haveBuff) + 100) / 100;
                    total = CalMulTotal(total, haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrMagic);
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalTotal(AttributeEnum.SpiritAtt, haveBuff, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) * (CalTotal(AttributeEnum.PanelSpiritAtt, haveBuff) + 100) / 100;
                    total = CalMulTotal(total, haveBuff, AttributeEnum.MulAttr, AttributeEnum.MulAttrSpirit);
                    break;
                case AttributeEnum.Def:
                    total = CalTotal(AttributeEnum.Def, haveBuff, AttributeEnum.DefIncrea) * (CalTotal(AttributeEnum.PanelDef, haveBuff) + 100) / 100;
                    total = CalMulTotal(total, haveBuff, AttributeEnum.MulDef);
                    break;
                case AttributeEnum.SecondExp:
                    total = CalTotal(AttributeEnum.SecondExp, haveBuff, AttributeEnum.ExpIncrea);
                    break;
                case AttributeEnum.SecondGold:
                    total = CalTotal(AttributeEnum.SecondGold, haveBuff, AttributeEnum.GoldIncrea);
                    break;
                default:
                    total = CalTotal(attrType, haveBuff);
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
            else
            {
                return CalMulTotal(1, false, attrType);
            }
        }


        public string GetPower()
        {
            double power = 0;

            Dictionary<int, GameAttribute> list = GameAttributeCategory.Instance.GetAll();


            foreach (int type in list.Keys)
            {
                double attrTotal = GetTotalAttr((AttributeEnum)type);
                float rate = list[type].PowerCoef;

                power += attrTotal * rate;
            }

            Debug.Log("Old Power:" + power);

            double p1 = GetTotalAttrDouble(AttributeEnum.PhyAtt);
            double p2 = GetTotalAttrDouble(AttributeEnum.MagicAtt);
            double p3 = GetTotalAttrDouble(AttributeEnum.SpiritAtt);

            double powerDamage = Math.Max(Math.Max(p1, p2), p3) * CalPercent(AttributeEnum.AurasAttrIncrea);
            powerDamage *= CalPercent(AttributeEnum.DamageIncrea) * CalPercent(AttributeEnum.AurasDamageIncrea);
            powerDamage *= (1 + GetTotalAttrDouble(AttributeEnum.Lucky) * 0.1);
            powerDamage *= Math.Min(GetTotalAttrDouble(AttributeEnum.CritRate), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamage) + 150) / 100;

            double powerDef = GetTotalAttrDouble(AttributeEnum.HP) / 10 + GetTotalAttrDouble(AttributeEnum.Def) * 3;
            powerDef *= CalPercent(AttributeEnum.DamageResist) * CalPercent(AttributeEnum.AurasDamageResist);
            powerDamage *= Math.Min(GetTotalAttrDouble(AttributeEnum.CritRateResist), 1) * (GetTotalAttrDouble(AttributeEnum.CritDamageResist) + 100) / 100;
            powerDef *= CalPercent(AttributeEnum.Miss);

            double newPower = powerDamage + powerDef;

            Debug.Log("New Power:" + newPower);

            return StringHelper.FormatNumber(power);
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
            }

            return total * (100.0 + percent) / 100.0;
        }

        private double CalMulTotal(double total, bool haveBuff, params AttributeEnum[] mulTypes)
        {
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
            }
            return total;
        }
    }
}