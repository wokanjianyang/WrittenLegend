using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class StoneConfigCategory
    {

    }

    public partial class StoneConfig
    {
        public int GetFee(int level)
        {
            return (int)(Math.Pow(2, level));
        }

        public int GetAttr(int level)
        {
            if (this.RiseType == 1)
            {
                return (int)MathHelper.GetSequence1(level) * this.AttrValue;
            }
            else
            {
                return level * this.AttrValue;
            }
        }
    }
}
