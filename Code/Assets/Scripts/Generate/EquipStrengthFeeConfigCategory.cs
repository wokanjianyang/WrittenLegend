using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipStrengthFeeConfigCategory
    {

        public EquipStrengthFeeConfig GetByLevel(long level)
        {
            try
            {
                return this.GetAll().Where(m => m.Value.StartLevel <= level && m.Value.EndLevel >= level).First().Value;
            }
            catch 
            {

            }

            return null;
        }
    }

}
