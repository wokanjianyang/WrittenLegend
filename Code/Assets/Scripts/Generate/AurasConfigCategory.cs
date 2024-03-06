using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AurasAttrConfigCategory
    {
        public AurasAttrConfig GetConfig(int aid)
        {
            var config = AurasAttrConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.AurasId == aid).FirstOrDefault();
            return config;
        }
    }

    public partial class AurasAttrConfig
    {

        public long GetAttr(long level)
        {
            long riseLevel = (level - 1);
            return this.AttrValue + riseLevel * this.Rise;
        }
    }


    public enum AurasType
    {
        AttrIncra = 1,
        AddDecrea = 2,
        AutoDamage = 3,
        AutoRestore = 4,
    }
}
