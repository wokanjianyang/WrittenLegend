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
            return (long)GetAttackAttrDouble(attrType);
        }

        public double GetTotalAttrDouble(AttributeEnum attrType)
        {
            double total = 0;

            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalTotal(AttributeEnum.HP, AttributeEnum.HpIncrea);
                    total = CalMulTotal(total, AttributeEnum.MulHp);
                    break;
                case AttributeEnum.PhyAtt:
                    total = CalTotal(AttributeEnum.PhyAtt, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea);
                    total = CalMulTotal(total, AttributeEnum.MulAttr, AttributeEnum.MulAttrPhy);
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalTotal(AttributeEnum.MagicAtt, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea);
                    total = CalMulTotal(total, AttributeEnum.MulAttr, AttributeEnum.MulAttrMagic);
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalTotal(AttributeEnum.SpiritAtt, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea);
                    total = CalMulTotal(total, AttributeEnum.MulAttr, AttributeEnum.MulAttrSpirit);
                    break;
                case AttributeEnum.Def:
                    total = CalTotal(AttributeEnum.Def, AttributeEnum.DefIncrea);
                    total = CalMulTotal(total, AttributeEnum.MulDef);
                    break;
                case AttributeEnum.SecondExp:
                    total = CalTotal(AttributeEnum.SecondExp, AttributeEnum.ExpIncrea);
                    break;
                case AttributeEnum.SecondGold:
                    total = CalTotal(AttributeEnum.SecondGold, AttributeEnum.GoldIncrea);
                    break;
                default:
                    total = CalTotal(attrType);
                    break;
            }

            return total;
        }

        public double GetAttackAttrDouble(AttributeEnum attrType)
        {
            double total = 0;
            switch (attrType)
            {
                case AttributeEnum.HP:
                    total = CalTotal(AttributeEnum.HP, AttributeEnum.HpIncrea) * (CalTotal(AttributeEnum.PanelHp) + 100) / 100;
                    break;
                case AttributeEnum.PhyAtt:
                    total = CalTotal(AttributeEnum.PhyAtt, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) * (CalTotal(AttributeEnum.PanelPhyAtt) + 100) / 100;
                    break;
                case AttributeEnum.MagicAtt:
                    total = CalTotal(AttributeEnum.MagicAtt, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) * (CalTotal(AttributeEnum.PanelMagicAtt) + 100) / 100;
                    break;
                case AttributeEnum.SpiritAtt:
                    total = CalTotal(AttributeEnum.SpiritAtt, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) * (CalTotal(AttributeEnum.PanelSpiritAtt) + 100) / 100;
                    break;
                case AttributeEnum.Def:
                    total = CalTotal(AttributeEnum.Def, AttributeEnum.DefIncrea) * (CalTotal(AttributeEnum.PanelDef) + 100) / 100;
                    break;
                case AttributeEnum.SecondExp:
                    total = CalTotal(AttributeEnum.SecondExp, AttributeEnum.ExpIncrea);
                    break;
                case AttributeEnum.SecondGold:
                    total = CalTotal(AttributeEnum.SecondGold, AttributeEnum.GoldIncrea);
                    break;
                default:
                    total = CalTotal(attrType);
                    break;
            }

            return total;
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

            return StringHelper.FormatNumber(power);
        }

        private double CalTotal(AttributeEnum type, params AttributeEnum[] increaTypes)
        {
            double total = 0;

            foreach (double hp in AllAttrDict[type].Values)
            {
                total += hp;
            }

            if (BuffDict.ContainsKey(type))
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

                if (BuffDict.ContainsKey(percentType))
                {
                    foreach (var item in BuffDict[percentType])
                    {
                        percent += item.AttrValue;
                    }
                }
            }

            return total * (100.0 + percent) / 100.0;
        }

        private double CalMulTotal(double total, params AttributeEnum[] mulTypes)
        {
            for (int i = 0; i < mulTypes.Length; i++)
            {
                AttributeEnum percentType = mulTypes[i];
                foreach (double pc in AllAttrDict[percentType].Values)
                {
                    total *= (100.0 + pc) / 100.0;
                }

                if (BuffDict.ContainsKey(percentType))
                {
                    foreach (var item in BuffDict[percentType])
                    {
                        total *= (100.0 + item.AttrValue) / 100.0;
                    }
                }
            }
            return total;
        }

        //private double CalTotal(AttributeEnum type)
        //{
        //    double total = 0;

        //    foreach (double attr in AllAttrDict[type].Values)
        //    {
        //        total += attr;
        //    }

        //    return total;
        //}
    }
}