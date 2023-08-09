using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class PhantomConfigCategory
    {
        public PhantomAttrConfig GetAttrConfig(int phid, int level)
        {
            var list = PhantomAttrConfigCategory.Instance.GetAll().Where(m => m.Value.PhId == phid && m.Value.Level == level).Select(m => m.Value).ToList();

            if (list.Count == 1)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }
    }

    
}
