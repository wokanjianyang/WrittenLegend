using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class InfiniteModelConfigCategory
    {
        public InfiniteModelConfig RandomConfig()
        {
            int i = RandomHelper.RandomNumber(0, this.list.Count);
            return this.list[i];
        }
    }

}
