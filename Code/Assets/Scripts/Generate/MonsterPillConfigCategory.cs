using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterPillConfigCategory
    {
        public MonsterPillConfig GetByTypeAndLayer(long type, int layer)
        {
            MonsterPillConfig config = this.list.Where(m => m.Type == type && m.Layer == layer).FirstOrDefault();

            return config;
        }


    }

}