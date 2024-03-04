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
}
