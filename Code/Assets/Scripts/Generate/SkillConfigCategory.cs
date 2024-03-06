using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillConfigCategory
    {
    }

    public partial class SkillConfig
    {
        public long GetMaxLevel(long level)
        {
            return this.MaxLevel + this.RiseMaxLevel * level;
        }
    }
}