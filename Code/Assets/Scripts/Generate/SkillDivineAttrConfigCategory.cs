using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SkillDivineAttrConfigCategory
    {

        public SkillDivineAttrConfig GetBySkillId(int skillId)
        {
            return this.list.Where(m => m.SkillId == skillId).FirstOrDefault();
        }
    }
}