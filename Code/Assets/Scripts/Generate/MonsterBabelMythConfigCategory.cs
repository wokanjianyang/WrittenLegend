using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MonsterBabelMythConfigCategory
    {
        public MonsterBabelMythConfig GetByProgress(long progress)
        {
            MonsterBabelMythConfig config = this.list.Where(m => m.StartLevel <= progress && progress <= m.EndLevel).FirstOrDefault();

            return config;
        }


    }

}