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

        public int GetTotalFee(long level)
        {
            int total = 0;
            for (int i = 1; i <= level; i++)
            {
                total += GetFee(i);
            }

            return total;
        }

        public int GetFee(int level)
        {
            int rise = Math.Min(level / 10, 2);
            return rise + 1;
        }
    }


    public partial class RelicConfig
    {
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
    public partial class RelicGroupConfig
    {
        public double GetAttrValue(long level)
        {
            if (level <= 0)
            {
                return 0;
            }
            else
            {
                if (this.RiseType == 2)
                {
                    return this.AttrValue * Math.Round(Math.Pow(this.RiseAttr, level - 1), 2);
                }
                else if (this.RiseType == 1)
                {
                    return this.AttrValue + this.RiseAttr * (level - 1);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}