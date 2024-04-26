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
                return this.list.Where(m => m.StartLevel <= level && level <= m.EndLevel).First();
            }
            catch
            {

            }

            return null;
        }
    }

    public partial class EquipRefineConfig
    {

        public long GetFee(long level)
        {
            if (this.BaseFee <= 0)
            {
                return MathHelper.GetSequence1(level) * this.RiseFee;
            }
            else
            {
                return this.BaseFee + (level - this.StartLevel) * this.RiseFee;
            }
        }

        public long GetBaseAttrPercent(long level)
        {
            return level;
        }

        public long GetQualityAttrPercent(long level)
        {
            return Math.Max(0, level - 49);
        }
    }

}
