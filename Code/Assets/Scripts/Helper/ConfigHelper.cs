using System.Text;

namespace Game
{
	public class ConfigHelper
	{
		public const long Max_Level = 500000; //最大等级

		public const double Def_Rate = 3.0; //防御系数

		public static int[] RuneRate = new int[] { 1, 3, 8, 18, 39, 85, 185 }; //词条产生概率，按等级

		public const long PackTime = 1690954379; //打包时间，防止作弊

		public const int CopyMax = 5; //副本最大累计次数

		public const int CopyMaxView = 6; //副本最大显示数量

		public const long MaxOfflineTime = 3600 * 12;
	}
}