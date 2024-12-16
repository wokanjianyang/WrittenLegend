using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipReformFeeConfigCategory
    {

        public EquipReformFeeConfig GetByLevel(long level)
        {
            try
            {
                return this.list.Where(m => m.StartLevel <= level && m.EndLevel >= level).First();
            }
            catch
            {

            }

            return null;
        }
    }

    public partial class EquipReformFeeConfig
    {
        public long GetFee(long level)
        {
            return BaseFee + (level - StartLevel) * RiseFee;
        }
    }

}
