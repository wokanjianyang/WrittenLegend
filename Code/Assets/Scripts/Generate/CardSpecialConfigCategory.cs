using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class CardSpecialConfigCategory
    {

    }

    public partial class CardSpecialConfig
    {

        public int GetFee(int level)
        {
            return  1;
        }

        public double GetAttrValue(int index, int level)
        {
            if (level <= 0)
            {
                return 0;
            }
            else
            {
                return this.AttrValueList[index] + this.AttrRiseList[index] * (level - 1);
            }
        }
    }

}
