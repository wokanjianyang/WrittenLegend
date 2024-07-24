using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class RingConfigCategory
    {
        public long ConvertOldToNew(long level)
        {
            long total = 0;

            for (int i = 0; i < level; i++)
            {
                total += GetOldNeedNumber(i);
            }

            return total;
        }

        private long GetOldNeedNumber(long level)
        {
            return Math.Min(level + 1, 3);
        }
    }

    public partial class RingConfig {

        public long GetAttr(int index, long level)
        {
            if (level < 1)
            {
                return 0;
            }

            return AttrValueList[index] + AttrRiseList[index] * (level - 1);
        }
    }

    public enum RingType
    {
        MB = 1,
        HT = 2,
        FH = 3,
        YS = 4,
        CS = 5,
        FY = 6,
    }
}
