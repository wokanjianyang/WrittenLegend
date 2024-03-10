using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MetalConfigCategory
    {

    }

    public partial class MetalConfig
    {
        public long GetNextRate(long level)
        {
            if (this.RisePower > 0)
            {
                long p = (long)Math.Log(level, this.RiseLog);
                return (long)Math.Pow(this.RiseLog, p + 1);
            }

            return 0;
        }

        public long GetRate(long level)
        {
            if (this.RisePower > 0)
            {
                long p = (long)Math.Log(level, this.RiseLog);
                return (long)Math.Pow(this.RisePower, p);
            }

            return 1;
        }

        public long GetAttr(long level)
        {
            long attr = this.AttrValue * level * GetRate(level);
            return attr;
        }
    }
}
