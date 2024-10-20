﻿using System.Text;

namespace Game
{
    public class ConfigHelper
    {
#if IS_TAPTAP
        public const int Channel = 1; //Tap 1 QQ 2
#else
        public const int Channel = 2; //Tap 1 QQ 2
#endif

        public const int Channel_Tap = 1;

        public const int Version = 301;

        public const long PackTime = 1729385954; //打包时间，防止作弊

        public const long PackEndTime = 1731113954; //超过此时间,游戏不能使用，需要更新

        public const long Max_Level = 120000; //最大人物等级和强化等级

        public const long Cycle_Level = 10000; //每次轮回增加等级

        public const long Cycle_Max = 11;

        public const long Max_Legacy_Level = 10; //最大传世副本等级

        public const long Max_Floor = 20000000; //最大闯关

        public const int FastFloor = 10000000;

        public const long RestoreGold = 5000000000000000L;

        public const double Def_Rate = 3.0; //防御系数

        public static int[] RuneRate = new int[] { 1, 3, 8, 18, 39, 85 }; //紫色一下装备产生概率
        public static int[] RuneRate1 = new int[] { 1, 3, 6, 10, 15, 20, 25 }; //0-350橙色装备产生概率 
        public static int[] RuneRate2 = new int[] { 1, 4, 9, 15, 25, 35, 45, 55 }; //350-650橙色装备产生概率 
        public static int[] RuneRate3 = new int[] { 1, 4, 9, 15, 25, 35, 45, 55, 60 }; //700以上橙色装备产生概率 

        public static int[] RuneRate99 = new int[] { 1, 3, 6, 9, 15, 24, 39, 59, 85 }; //专属产生概率

        public const long MaxOfflineTime = 3600 * 24;  //最长离线时间

        //public const int MaxBagCount = 210;  // 包裹数量
        public static int[] BagCount = new int[] { 100, 100, 100, 350, 350 };

        public const int CopyTicketCd = 900; //15分钟
        public const int CopyTicketCdMin = 120; //2分钟
        public const int LegacyTicketCd = 10800; //1小时 

        public const int CopyTicketMax = 1500; //副本最大次数
        public const int CopyTicketFirstCount = 300; //副本离线和新号最多多少次
        public const int LegacyTiketMax = 50; //传世挑战最大次数

        public static int[] PercentAttrIdList = { 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 23, 24, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 41, 43, 45, 101, 102, 103, 110, 201, 202, 203, 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011 };

        public static int[] RateAttrIdList = { 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009 };

        public static string[] LayerNameList = { "黄", "玄", "地", "天", "荒", "洪", "宙", "宇", "极", "道", "虚", "始" };
        public static string[] LayerChinaList = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十",
            "十一", "十二", "十三", "十四", "十五", "十六", "十七", "十八","十九", "二十",
         "二一", "二二", "二三", "二四", "二五", "二六", "二七", "二八","二九", "三十",};

        public const int MapStartId = 1000;

        public const int DefendHp = 2000; //防守塔默认血量
        public const int DefendMaxLevel = 4; //守沙难度

        public const int SkillSuitMax = 4;
        public const int SkillSuitMin = 2;

        public const int SkillNumber = 5;

        public const float DelayShowTime = 0.75f;
        //public const float SkillAnimaTime = 0.75f;
        //public const float SkillAnimaTime1 = 0.75f;

        public const float PvpRate = 200;
        public const float ValetPvpRate = 30;

        public const int EquipRefreshCount = 10;

        public const int AutoExitMapTime = 2;
        public const int AutoStartMapTime = 2;
        public const int AutoResurrectionTime = 10;

        public const int PillDefaultTime = 60;
        public const int BabelCount = 300;

        public const int Mine_Time = 60;

        public const int Infinit_Max = 2000;

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