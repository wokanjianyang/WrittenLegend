﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class AttributeBonus
	{
		private Dictionary<AttributeEnum, Dictionary<AttributeFrom, long>> AllAttrDict = new Dictionary<AttributeEnum, Dictionary<AttributeFrom, long>>();

		public AttributeBonus(){
			foreach (AttributeEnum item in Enum.GetValues(typeof(AttributeEnum)))
			{
				AllAttrDict.Add(item, new Dictionary<AttributeFrom, long>());
			}

		}

		public void SetAttr(AttributeEnum attrType, AttributeFrom attrKey, long attrValue)
		{
			AllAttrDict[attrType][attrKey] = attrValue;
		}

		public long GetTotalAttr(AttributeEnum attrType)
		{
			switch (attrType)
			{
				case AttributeEnum.HP:
					return CalTotal(AttributeEnum.HP, AttributeEnum.HpIncrea);
				case AttributeEnum.PhyAtt:
					return CalTotal(AttributeEnum.PhyAtt, AttributeEnum.AttIncrea);
				case AttributeEnum.MagicAtt:
					return CalTotal(AttributeEnum.MagicAtt, AttributeEnum.AttIncrea);
				case AttributeEnum.SpiritAtt:
					return CalTotal(AttributeEnum.SpiritAtt, AttributeEnum.AttIncrea);
				case AttributeEnum.Def:
					return CalTotal(AttributeEnum.Def, AttributeEnum.DefIncrea);
				default:
					return CalTotal(attrType);
			}
		}

		private long CalTotal(AttributeEnum type, AttributeEnum typeIncrea) {
			long total = 0;

			foreach (long hp in AllAttrDict[type].Values) {
				total += hp;
			}

			long percent = 0;
			foreach (long pc in AllAttrDict[typeIncrea].Values) {
				percent += pc;
			}

            return total * (100+ percent) /100;
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
	}
}