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

            SkillRune rune = new SkillRune();
            rune.SkillId = config.SkillId;
            rune.Name = config.Name;
            rune.Des = config.Des;
            rune.Incre = config.Percent;
            rune.Max = config.Max;
            rune.SuitId = config.SuitId;

            return config.Id;
        }
    }
}
