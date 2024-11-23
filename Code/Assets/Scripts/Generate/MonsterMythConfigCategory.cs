using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterMythConfigCategory
    {
        public MonsterMythConfig GetByMapIdAndQuality(long mapId, int layer)
        {
            MonsterMythConfig config = this.list.Where(m => m.MapId == mapId && m.Quality == layer).FirstOrDefault();

            return config;
        }


    }

}