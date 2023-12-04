using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class WingConfigCategory
    {
        public WingConfig GetByLevel(long level)
        {
            var config = WingConfigCategory.Instance.GetAll().Where(m => m.Value.Level == level).Select(m => m.Value).FirstOrDefault();
            return config;
        }
    }
}
