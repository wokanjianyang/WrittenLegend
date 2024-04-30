using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBaseCategory
    {
        public Monster BuildMonster(MapConfig mapConfig, int quality, int rate, int modelId, RuleType ruleType)
        {
            MonsterBase config = this.GetByMapId(mapConfig.Id);

            Monster enemy = new Monster(mapConfig.Id, config.Id, quality, rate, modelId, ruleType);
            return enemy;
        }

        public MonsterBase GetByMapId(int MapId)
        {
            return this.list.Where(m => m.MapId == MapId).First();
        }
    }

}