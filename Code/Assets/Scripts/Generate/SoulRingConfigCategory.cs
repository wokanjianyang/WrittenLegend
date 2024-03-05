using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SoulRingConfigCategory
    {
        public SoulRingAttrConfig GetAttrConfig(int sid, long level)
        {
            var config = SoulRingAttrConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.Sid == sid
            && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
            return config;
        }
    }

    public partial class SoulRingAttrConfig
    {
        public long GetFee(long level) {
            long riseLevel = (level - this.StartLevel);
            long fee = this.Fee + riseLevel * this.Fee;
            return fee;
        }

        public long GetAttr(int index, long level)
        {
            long riseLevel = (level - this.StartLevel);
            long attr = this.AttrValueList[index] + riseLevel * this.AttrRiseList[index];
            return attr;
        }
    }
}
