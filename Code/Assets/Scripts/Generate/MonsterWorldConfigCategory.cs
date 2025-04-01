using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterWorldConfigCategory
    {
        public MonsterWorldConfig GetByMapIdAndStep(long mapId, int step)
        {
            MonsterWorldConfig config = this.list.Where(m => m.MapId == mapId && m.Step == step).FirstOrDefault();

            return config;
        }


    }

}