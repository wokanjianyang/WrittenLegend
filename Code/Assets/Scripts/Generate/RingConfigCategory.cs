using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class RingConfigCategory
    {

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
