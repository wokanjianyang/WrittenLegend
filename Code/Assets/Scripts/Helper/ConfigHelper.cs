﻿using System.Text;

namespace Game
{
	public class ConfigHelper
	{
		public const long Max_Level = 2000000; //最大等级

		public const double Def_Rate = 3.0; //防御系数

		public static int[] RuneRate = new int[] { 1, 3, 8, 18, 39, 85, 185 }; //词条产生概率，按等级

		public const long PackTime = 1690954379; //打包时间，防止作弊

		//public const int CopyMax = 5; //副本最大累计次数

		//public const int CopyMaxView = 10; //副本最大显示数量

		public const long MaxOfflineTime = 3600 * 12;  //最长离线时间

		public const int MaxBagCount = 300;  // 包裹数量

		public const int CopyTicketCd = 900; //15分钟

		public const int CopyTicketFirstCount = 100; //离线和新号最多多少次

		public static int[] PercentAttrIdList = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 24, 41, 43, 45 };
	}
}