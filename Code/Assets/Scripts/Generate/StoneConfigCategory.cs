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
        private const int FeeRate = 2;

        public int GetFee(int level)
        {
            return (int)(MathHelper.GetSequence1(level) * FeeRate);
        }

        public int GetAttr(int level)
        {
            if (this.RiseType == 1)
            {
                return (int)(MathHelper.GetSequence1(level) + this.AttrValue * level);
            }
            else
            {
                return level * this.AttrValue;
            }
        }
    }
}
