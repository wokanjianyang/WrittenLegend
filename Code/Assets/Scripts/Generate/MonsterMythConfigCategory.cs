using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterMythConfigCategory
    {
        public MonsterMythConfig GetByMapIdAndLayer(long mapId, int layer)
        {
            MonsterMythConfig config = this.list.Where(m => m.MapId <= mapId && m.Layer <= layer).FirstOrDefault();

            return config;
        }


    }

}