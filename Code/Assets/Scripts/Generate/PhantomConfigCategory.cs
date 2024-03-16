using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class PhantomConfigCategory
    {
        public PhantomAttrConfig GetAttrConfig(int phid, int level)
        {
            PhantomAttrConfig config = PhantomAttrConfigCategory.Instance.GetAll().Select(m => m.Value)
                .Where(m => m.PhId == phid && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
            return config;
        }
    }

    public partial class PhantomAttrConfig
    {
        public double GetAttrRate(int level)
        {
            double rate = 1;
            for (int i = 1; i < level; i++)
            {
                rate *= this.AttrRise;
            }

            return rate;
        }

        public double GetAttrAdvanceRate(int level)
        {
            return (level - 1) * this.AttrAdvanceRise; ;
        }

        public int GetRewardAttr(int level)
        {
            return this.RewardBase + (level - 1) * this.RewardRise;
        }
    }
}
