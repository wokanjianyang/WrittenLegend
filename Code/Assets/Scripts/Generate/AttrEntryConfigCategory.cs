using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AttrEntryConfigCategory
    {
        public void Build(ref Equip equip)
        {
            int level = equip.Level;

            List<AttrEntryConfig> configs = list.FindAll(m => m.LevelRequired == level);

            if (configs.Count <= 0) {
                return;
            }

            int rd = RandomHelper.RandomNumber(0, configs.Count);

            if (equip.AttrEntryList == null) {
                equip.AttrEntryList = new List<KeyValuePair<int, long>>();
            }

            equip.AttrEntryList.Add(new KeyValuePair<int, long>(configs[rd].AttrId, configs[rd].Val));
        }
    }
}
