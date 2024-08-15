using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBabelConfigCategory
    {
        public MonsterBabelConfig GetByProgress(long progress)
        {
            MonsterBabelConfig config = this.list.Where(m => m.StartLevel <= progress && progress <= m.EndLevel).FirstOrDefault();

            return config;
        }


    }


    public partial class MonsterBabelConfig
    {
        public int[] GetSkillIdList(long progress)
        {
            if (progress % 100 == 0)
            {
                return this.SkillIdList3;
            }
            else if (progress % 10 == 0)
            {

                return this.SkillIdList2;
            }
            else
            {
                return this.SkillIdList1;
            }
        }
    }

}