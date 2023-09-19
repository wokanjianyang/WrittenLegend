using System.Text;

namespace Game
{
	public class ConfigHelper
	{
		public const int Version = 146;

		public const long Max_Level = 15000; //最大人物等级和强化等级

		public const long Max_Level_Refine = 150; //最大精练等级

		public const double Def_Rate = 3.0; //防御系数

		public static int[] RuneRate = new int[] { 1, 3, 8, 18, 39, 85, 185 }; //词条产生概率，按等级
		                             
		public const long PackTime = 1695094373; //打包时间，防止作弊

		public const long PackEndTime = 1695699173; //超过此时间,游戏不能使用，需要更新

		//public const int CopyMax = 5; //副本最大累计次数

		//public const int CopyMaxView = 10; //副本最大显示数量

		public const long MaxOfflineTime = 3600 * 12;  //最长离线时间

		public const int MaxBagCount = 150;  // 包裹数量

		public const int CopyTicketCd = 900; //15分钟

		public const int CopyTicketMax = 200; //最大次数
		public const int CopyTicketFirstCount = 100; //离线和新号最多多少次

		public static int[] PercentAttrIdList = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 24, 26, 27, 28, 41, 43, 45 };

		public const int MapStartId = 1000;

		public const int SkillSuitMax = 4;
		public const int SkillSuitMin = 2;

		public const int SkillNumber = 5;

		public const float DelayShowTime = 0.5f;
	}
}