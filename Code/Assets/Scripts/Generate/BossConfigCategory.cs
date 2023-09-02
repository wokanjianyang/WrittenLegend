using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class BossConfigCategory
    {

    }

    public class BossHelper
    {
        public static Boss BuildBoss(int bossId, int mapId, int copyType, int rate)
        {
            Boss boss = new Boss(bossId, mapId, copyType, rate);
            return boss;
        }
    }
}