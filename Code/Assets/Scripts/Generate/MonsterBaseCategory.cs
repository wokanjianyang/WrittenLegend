using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBaseCategory
    {

    }

    public class MonsterHelper
    {
        public static Monster BuildMonster(MapConfig mapConfig, int quality, int rate, int modelId,RuleType ruleType)
        {
            MonsterBase config = MonsterBaseCategory.Instance.GetAll().Where(m => m.Value.MapId == mapConfig.Id).First().Value;

            Monster enemy = new Monster(config.Id, quality, rate, modelId,ruleType);
            return enemy;
        }
    }
}