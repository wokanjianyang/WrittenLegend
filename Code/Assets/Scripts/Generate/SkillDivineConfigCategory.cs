using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillDivineConfigCategory
    {
        public SkillDivineConfig GetConfig(int id, int level)
        {
            if (level > 4) return null;
            else return this.Get(id);
        }
    }
}