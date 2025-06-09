using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class FashionSuitConfigCategory
    {

    }


    public partial class FashionSuitConfig
    {

        public long GetAttrValue(int index, long suitLevel)
        {
            if (suitLevel <= 0)
            {
                return 0;
            }
            else
            {
                return this.AttrValueList[index] + this.AttrRiseList[index] * (suitLevel - 1);
            }
        }
    }



}