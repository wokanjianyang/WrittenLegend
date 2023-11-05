using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipRefineConfigCategory
    {

        public EquipRefineConfig GetByLevel(long level)
        {
            try
            {
                return this.GetAll().Where(m => m.Value.Level == level).First().Value;
            }
            catch 
            {

            }

            return null;
        }
    }

}
