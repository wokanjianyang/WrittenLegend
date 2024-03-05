using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class WingConfigCategory
    {
        public WingConfig GetByLevel(long level)
        {
            var config = WingConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
            return config;
        }
    }

    public partial class WingConfig
    {
        public long GetFee(long level)
        {
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
