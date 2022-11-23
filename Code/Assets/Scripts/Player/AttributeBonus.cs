using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class AttributeBonus
	{
		private Dictionary<int, long> HpDict = new Dictionary<int, long>();
		private Dictionary<int, long> HpDictPercent = new Dictionary<int, long>();
		private Dictionary<int, long> PhyAttrDict = new Dictionary<int, long>();
		private Dictionary<int, long> PhyAttrPercent = new Dictionary<int, long>();

		public void SetAttr(AttributeEnum attrType,int attrKey, long attrValue)
		{
			switch (attrType)
			{
				case AttributeEnum.HP:
					HpDict[attrKey] = attrValue;
					break;
				case AttributeEnum.PhyAtt:
					PhyAttrDict[attrKey] = attrValue;
                    break;
				case AttributeEnum.AttIncrea:
					PhyAttrPercent[attrKey] = attrValue;
					break;
			}

		}

		public long GetTotalAttr(AttributeEnum attrType)
		{
			switch (attrType)
			{
				case AttributeEnum.HP:
					return GetHpTotal();
				case AttributeEnum.PhyAtt:
					return GetPhyAttrTotal();
			}
			return 0;
		}

		private long GetHpTotal() {
			long total = 0;

			foreach (long hp in HpDict.Values) {
				total += hp;
			}

			long percent = 0;
			foreach (long pc in HpDictPercent.Values) {
				percent += pc;
			}

            return total * (100+ percent) /100;
		}

		private long GetPhyAttrTotal()
		{
			long total = 0;

			foreach (long attr in PhyAttrDict.Values)
			{
				total += attr;
			}

			long percent = 0;
			foreach (long pc in PhyAttrPercent.Values)
			{
				percent += pc;
			}

			return total * (100 + percent) / 100;
		}
	}
}