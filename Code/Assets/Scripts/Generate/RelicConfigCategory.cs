using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class RelicConfigCategory
    {
        public List<RelicConfig> GetListByType(int type)
        {
            return this.list.Where(m => m.Type == type).ToList();
        }
    }

}