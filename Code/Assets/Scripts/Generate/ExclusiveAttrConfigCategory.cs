using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ExclusiveAttrConfigCategory
    {
        public ExclusiveAttrConfig GetByLevel(int level)
        {
            return this.list.Where(m => m.Level == level).FirstOrDefault();
        }
    }
}