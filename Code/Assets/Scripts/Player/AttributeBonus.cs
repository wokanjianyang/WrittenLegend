﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Game
{
    public class AttributeBonus
    {
        private Dictionary<AttributeEnum, Dictionary<int, long>> AllAttrDict = new Dictionary<AttributeEnum, Dictionary<int, long>>();

        public AttributeBonus()
        {
            foreach (AttributeEnum item in Enum.GetValues(typeof(AttributeEnum)))
            {
                AllAttrDict.Add(item, new Dictionary<int, long>());
            }

        }

        public void SetAttr(AttributeEnum attrType, AttributeFrom attrKey, long attrValue)
        {
            int key = (int)attrKey;
            AllAttrDict[attrType][key] = attrValue;
        }

        public void SetAttr(AttributeEnum attrType, int attrKey, long attrValue)
        {
            AllAttrDict[attrType][attrKey] = attrValue;
        }


        public void SetAttr(AttributeEnum attrType, AttributeFrom attrKey, int Position, long attrValue)
        {
            int key = ((int)attrKey) * 9999 + Position;
            AllAttrDict[attrType][key] = attrValue;
        }

        public BigInteger GetTotalAttrBig(AttributeEnum attrType) {
            switch (attrType)
            {
                case AttributeEnum.HP:
                    return CalTotalBig(AttributeEnum.HP, AttributeEnum.HpIncrea);
                case AttributeEnum.PhyAtt:
                    return CalTotalBig(AttributeEnum.PhyAtt, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea);
                case AttributeEnum.MagicAtt:
                    return CalTotalBig(AttributeEnum.MagicAtt, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea);
                case AttributeEnum.SpiritAtt:
                    return CalTotalBig(AttributeEnum.SpiritAtt, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea);
                case AttributeEnum.Def:
                    return CalTotalBig(AttributeEnum.Def, AttributeEnum.DefIncrea);
                case AttributeEnum.SecondExp:
                    return CalTotalBig(AttributeEnum.SecondExp, AttributeEnum.ExpIncrea);
                case AttributeEnum.SecondGold:
                    return CalTotalBig(AttributeEnum.SecondGold, AttributeEnum.GoldIncrea);
                default:
                    return CalTotalBig(attrType);
            }
        }

        public long GetTotalAttr(AttributeEnum attrType)
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

        public long GetAttackAttr(AttributeEnum attrType)
        {
            switch (attrType)
            {
                case AttributeEnum.HP:
                    return CalTotal(AttributeEnum.HP, AttributeEnum.HpIncrea) * (CalTotal(AttributeEnum.PanelHp) + 100) / 100;
                case AttributeEnum.PhyAtt:
                    return CalTotal(AttributeEnum.PhyAtt, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) * (CalTotal(AttributeEnum.PanelPhyAtt) + 100) / 100;
                case AttributeEnum.MagicAtt:
                    return CalTotal(AttributeEnum.MagicAtt, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) * (CalTotal(AttributeEnum.PanelMagicAtt) + 100) / 100;
                case AttributeEnum.SpiritAtt:
                    return CalTotal(AttributeEnum.SpiritAtt, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) * (CalTotal(AttributeEnum.PanelSpiritAtt) + 100) / 100;
                case AttributeEnum.Def:
                    return CalTotal(AttributeEnum.Def, AttributeEnum.DefIncrea) * (CalTotal(AttributeEnum.PanelDef) + 100) / 100;
                case AttributeEnum.SecondExp:
                    return CalTotal(AttributeEnum.SecondExp, AttributeEnum.ExpIncrea);
                case AttributeEnum.SecondGold:
                    return CalTotal(AttributeEnum.SecondGold, AttributeEnum.GoldIncrea);
                default:
                    return CalTotal(attrType);
            }
        }

        public BigInteger GetAttackAttrBig(AttributeEnum attrType)
        {
            switch (attrType)
            {
                case AttributeEnum.HP:
                    return CalTotalBig(AttributeEnum.HP, AttributeEnum.HpIncrea) * (CalTotal(AttributeEnum.PanelHp) + 100) / 100;
                case AttributeEnum.PhyAtt:
                    return CalTotalBig(AttributeEnum.PhyAtt, AttributeEnum.AttIncrea, AttributeEnum.PhyAttIncrea) * (CalTotal(AttributeEnum.PanelPhyAtt) + 100) / 100;
                case AttributeEnum.MagicAtt:
                    return CalTotalBig(AttributeEnum.MagicAtt, AttributeEnum.AttIncrea, AttributeEnum.MagicAttIncrea) * (CalTotal(AttributeEnum.PanelMagicAtt) + 100) / 100;
                case AttributeEnum.SpiritAtt:
                    return CalTotalBig(AttributeEnum.SpiritAtt, AttributeEnum.AttIncrea, AttributeEnum.SpiritAttIncrea) * (CalTotal(AttributeEnum.PanelSpiritAtt) + 100) / 100;
                case AttributeEnum.Def:
                    return CalTotalBig(AttributeEnum.Def, AttributeEnum.DefIncrea) * (CalTotal(AttributeEnum.PanelDef) + 100) / 100;
                case AttributeEnum.SecondExp:
                    return CalTotalBig(AttributeEnum.SecondExp, AttributeEnum.ExpIncrea);
                case AttributeEnum.SecondGold:
                    return CalTotalBig(AttributeEnum.SecondGold, AttributeEnum.GoldIncrea);
                default:
                    return CalTotalBig(attrType);
            }
        }

        public string GetPower()
        {
            long power = 0;

            Dictionary<int, GameAttribute> list = GameAttributeCategory.Instance.GetAll();


            foreach (int type in list.Keys)
            {
                long attrTotal = GetTotalAttr((AttributeEnum)type);
                float rate = list[type].PowerCoef;

                power += (long)(attrTotal * rate);
            }



            return StringHelper.FormatNumber(power);
        }

        private long CalTotal(AttributeEnum type, params AttributeEnum[] increaTypes)
        {
            long total = 0;

            foreach (long hp in AllAttrDict[type].Values)
            {
                total += hp;
            }

            long percent = 0;

            for (int i = 0; i < increaTypes.Length; i++)
            {
                AttributeEnum percentType = increaTypes[i];
                foreach (long pc in AllAttrDict[percentType].Values)
                {
                    percent += pc;
                }
            }

            return total * (100 + percent) / 100;
        }

        private long CalTotal(AttributeEnum type)
        {
            long total = 0;

            foreach (long attr in AllAttrDict[type].Values)
            {
                total += attr;
            }

            return total;
        }


        private BigInteger CalTotalBig(AttributeEnum type, params AttributeEnum[] increaTypes)
        {
            BigInteger total = 0;

            foreach (long hp in AllAttrDict[type].Values)
            {
                total += hp;
            }

            long percent = 0;

            for (int i = 0; i < increaTypes.Length; i++)
            {
                AttributeEnum percentType = increaTypes[i];
                foreach (long pc in AllAttrDict[percentType].Values)
                {
                    percent += pc;
                }
            }

            return total * (100 + percent) / 100;
        }

        private BigInteger CalTotalBig(AttributeEnum type)
        {
            BigInteger total = 0;

            foreach (long attr in AllAttrDict[type].Values)
            {
                total += attr;
            }

            return total;
        }
    }
}