using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class RelicConfigCategory
    {
        public List<RelicConfig> GetListByType(int type)
        {
            return this.list.Where(m => m.Type == type).ToList();
        }
    }


    public partial class RelicConfig
    {
        public int GetFee(int level)
        {
            int rise = level / 10;
            return rise + 1;
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