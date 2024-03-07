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


        public long GetAttr(long level)
        {
            long rate = 1;

            if (this.RisePower > 0)
            {
                int p = (int)Math.Sqrt(level);

            }


            long attr = this.AttrValue * level * rate;
            return attr;
        }
    }
}
