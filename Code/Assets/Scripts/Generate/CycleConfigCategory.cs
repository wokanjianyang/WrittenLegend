using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class CycleConfigCategory
    {
        public CycleConfig GetByCycle(long type, long cycle)
        {
            return this.list.Where(m => m.Type == type && m.Cycle == cycle).FirstOrDefault();
        }

        public void Init()
        {
            this.list = new List<CycleConfig>();
        }
    }


}
