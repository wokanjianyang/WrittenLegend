﻿using System.Text;

namespace Game
{
    public class ConfigHelper
    {
        public const int Version = 164;

        public const long Max_Level = 40000; //最大人物等级和强化等级

        public const long Max_Level_Refine = 200; //最大精练等级

        public const long Max_Floor = 8000000; //最大闯关

        public const double Def_Rate = 3.0; //防御系数

        public static int[] RuneRate = new int[] { 1, 3, 8, 18, 39, 85, 185 }; //紫色一下装备产生概率
        public static int[] RuneRate1 = new int[] { 1, 3, 6, 9, 15, 24, 33, 42 }; //橙色装备产生概率
        public static int[] RuneRate2 = new int[] { 1, 3, 6, 9, 15, 24, 39 }; //专属产生概率

        public const long PackTime = 1702087310; //打包时间，防止作弊

        public const long PackEndTime = 1703124110; //超过此时间,游戏不能使用，需要更新

        public const long MaxOfflineTime = 3600 * 24;  //最长离线时间

        public const int MaxBagCount = 210;  // 包裹数量

        public const int CopyTicketCd = 900; //15分钟

        public const int CopyTicketMax = 200; //副本最大次数
        public const int CopyTicketFirstCount = 100; //副本离线和新号最多多少次

        public static int[] PercentAttrIdList = { 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 24, 26, 27, 28, 29, 30, 41, 43, 45 };

        public const int MapStartId = 1000;

        public const int DefendHp = 500; //防守塔默认血量

        public const int SkillSuitMax = 4;
        public const int SkillSuitMin = 2;

        public const int SkillNumber = 5;

        public const int FastFloor = 4000000;

        public const float DelayShowTime = 0.75f;
        //public const float SkillAnimaTime = 0.75f;
        //public const float SkillAnimaTime1 = 0.75f;

        public const float PvpRate = 100;

        public static int GetFloorRate(long floor)
        {
            if (floor > 100 && floor < FastFloor)
            {
                return 2;
            }
            return 1;
        }
    }
}