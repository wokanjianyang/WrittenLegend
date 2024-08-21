using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBabelConfigCategory
    {
        public MonsterBabelConfig GetByProgressAndType(long progress, int type)
        {
            MonsterBabelConfig config = this.list.Where(m => m.StartLevel <= progress && progress <= m.EndLevel && m.Type == type).FirstOrDefault();

            return config;
        }


    }

}