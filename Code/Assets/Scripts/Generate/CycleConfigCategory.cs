using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class CycleConfigCategory
    {
        public CycleConfig GetByCycle(long cycle)
        {
            return this.list.Where(m => m.Cycle == cycle).FirstOrDefault();
        }
    }


}
