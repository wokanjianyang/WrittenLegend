using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class TalentConfigCategory
    {

    }

    public partial class TalentConfig
    {
        public double GetAttrValue(long level)
        {
            if (level <= 0)
            {
                return 0;
            }

            double bv = this.AttrValue;
            for (int i = 1; i < level; i++)
            {
                if (this.RiseType == 1)
                {
                    bv += this.RiseValue;
                }
                else
                {
                    bv *= this.RiseValue;
                }
            }

            return bv;
        }
    }
}
