using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ExclusiveAttrConfigCategory
    {
        public ExclusiveAttrConfig GetAttr(int cycle, int level)
        {
            return this.list.Where(m => m.Cycle == cycle && m.Level == level).FirstOrDefault();
        }
    }
}