using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class PillConfigCategory
    {
        public PillConfig GetByLevel(long level)
        {
            long p = level % 2000;
            int type = p % 10 == 0 ? 2 : 1;
            long position = p / 100 + 1;

            var config = this.list.Where(m => m.Type == type && m.Position == position).FirstOrDefault();
            return config;
        }
    }

    public partial class PillConfig
    {

    }
}
