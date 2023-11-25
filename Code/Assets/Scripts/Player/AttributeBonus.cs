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

        public AttributeBonus()
        {
            foreach (AttributeEnum item in Enum.GetValues(typeof(AttributeEnum)))
            {
                AllAttrDict.Add(item, new Dictionary<int, double>());
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
            switch (attrType)
            {
                case AttributeEnum.HP:
                    return CalTotal(AttributeEnum.HP, AttributeEnum.HpIncrea);
                case AttributeEnum.PhyAtt:
                    return CalTotal(AttributeEnum.PhyAtt, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea);
                case AttributeEnum.MagicAtt:
                    return CalTotal(AttributeEnum.MagicAtt, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea);
                case AttributeEnum.SpiritAtt:
                    return CalTotal(AttributeEnum.SpiritAtt, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea);
                case AttributeEnum.Def:
                    return CalTotal(AttributeEnum.Def, AttributeEnum.DefIncrea);
                case AttributeEnum.SecondExp:
                    return CalTotal(AttributeEnum.SecondExp, AttributeEnum.ExpIncrea);
                case AttributeEnum.SecondGold:
                    return CalTotal(AttributeEnum.SecondGold, AttributeEnum.GoldIncrea);
                default:
                    return CalTotal(attrType);
            }
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

            double percent = 0;

            for (int i = 0; i < increaTypes.Length; i++)
            {
                AttributeEnum percentType = increaTypes[i];
                foreach (double pc in AllAttrDict[percentType].Values)
                {
                    percent += pc;
                }
            }

            return total * (100 + percent) / 100;
        }

        private double CalTotal(AttributeEnum type)
        {
            double total = 0;

            foreach (double attr in AllAttrDict[type].Values)
            {
                total += attr;
            }

            return total;
        }
    }
}