using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SoulRingConfigCategory
    {
        public SoulRingAttrConfig GetAttrConfig(int sid, long level)
        {
            var config = SoulRingAttrConfigCategory.Instance.GetAll().Where(m => m.Value.Sid == sid && m.Value.Level == level).Select(m => m.Value).FirstOrDefault();
            return config;
        }
    }
}
