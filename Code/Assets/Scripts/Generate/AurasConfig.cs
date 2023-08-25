using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AurasConfigCategory
    {
        public AurasAttrConfig GetAttrConfig(int aid, long level)
        {
            var config = AurasAttrConfigCategory.Instance.GetAll().Where(m => m.Value.AurasId == aid && m.Value.Level == level).Select(m => m.Value).FirstOrDefault();
            return config;
        }
    }

    public enum AurasType { 
        AttrIncra =1,
        AddDecrea = 2,
        AutoDamage = 3,
        AutoRestore = 4,
    }
}
