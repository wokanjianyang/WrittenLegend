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
        public static Boss BuildBoss(int bossId, int mapId, RuleType ruleType, int rewarCount, int modelId)
        {
            Boss boss = new Boss(bossId, mapId, ruleType, rewarCount, modelId);
            return boss;
        }
    }
}