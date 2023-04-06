using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillRuneConfigCategory
    {
        public int Build()
        {
            int index = RandomHelper.RandomNumber(0, list.Count);
            SkillRuneConfig config = list[index];
            return config.Id;
        }
    }
}
