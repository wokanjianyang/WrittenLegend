using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillConfigCategory
    {
    }

    public class SkillHelper
    {
        public static SkillBook BuildItem(int ConfigId)
        {
            SkillBook item = new SkillBook(ConfigId);
            return item;
        }
    }
}