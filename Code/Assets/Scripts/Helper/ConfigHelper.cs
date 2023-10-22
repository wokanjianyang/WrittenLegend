using System.Text;

namespace Game
{
	public class ConfigHelper
	{
		public const int Version = 154;

		public const long Max_Level = 20000; //最大人物等级和强化等级

		public const long Max_Level_Refine = 200; //最大精练等级

		public const long Max_Floor = 4000000; //最大闯关

		public const double Def_Rate = 3.0; //防御系数

		public static int[] RuneRate = new int[] { 1, 3, 8, 18, 39, 85, 185 }; //词条产生概率
		public static int[] RuneRate1 = new int[] { 1, 3, 6, 9, 15, 24, 39 }; //橙色和专属产生概率

		public const long PackTime = 1697959930; //打包时间，防止作弊

		public const long PackEndTime = 1698996730; //超过此时间,游戏不能使用，需要更新

		public const long MaxOfflineTime = 3600 * 12;  //最长离线时间

		public const int MaxBagCount = 210;  // 包裹数量

		public const int CopyTicketCd = 900; //15分钟

		public const int CopyTicketMax = 200; //副本最大次数
		public const int CopyTicketFirstCount = 100; //副本离线和新号最多多少次

		public static int[] PercentAttrIdList = { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 24, 26, 27, 28, 41, 43, 45 };

		public const int MapStartId = 1000;

		public const int SkillSuitMax = 4;
		public const int SkillSuitMin = 2;

		public const int SkillNumber = 5;

		public const float DelayShowTime = 0.75f;
		public const float SkillAnimaTime = 0.75f;
		public const float SkillAnimaTime1 = 0.75f;
	}
}