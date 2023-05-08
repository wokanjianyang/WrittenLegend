using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillRuneConfigCategory
    {

    }

    public class SkillRuneHelper {
        public static SkillRuneConfig RandomRune()
        {
            List<SkillRuneConfig> list = SkillRuneConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
            int index = RandomHelper.RandomNumber(0, list.Count);
            return list[index];
        }
    }
}
