using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillDoubleHitConfigCategory
    {
        public static SkillDoubleHitConfig RandomConfig()
        {
            List<SkillDoubleHitConfig> list = SkillDoubleHitConfigCategory.Instance.GetAll().Select(m => m.Value).ToList();
            int index = RandomHelper.RandomNumber(0, list.Count);
            return list[index];
        }
    }
}