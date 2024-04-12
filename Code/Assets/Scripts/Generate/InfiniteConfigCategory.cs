using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class InfiniteConfigCategory
    {
        public InfiniteConfig GetByLevel(long level)
        {
            InfiniteConfig config = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).First();
            return config;
        }
    }

}
