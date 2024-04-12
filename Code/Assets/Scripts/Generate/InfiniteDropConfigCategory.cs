using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class InfiniteDropConfigCategory
    {
        public List<InfiniteDropConfig> GetLevelList(long level)
        {
            List<InfiniteDropConfig> configs = this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).ToList();
            return configs;
        }
    }

}
