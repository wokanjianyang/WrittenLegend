using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class AttrEntryConfigCategory
    {
        public KeyValuePair<int, long> Build(int part, int level, int quality)
        {
            List<AttrEntryConfig> configs = list.FindAll(m => m.PartList.Contains(part) && m.MinLevel <= level && level <= m.MaxLevel && quality <= m.MaxQuality);

            if (configs.Count <= 0)
            {
                return new KeyValuePair<int, long>(0, 0);
            }

            int rd = RandomHelper.RandomNumber(0, configs.Count);

            AttrEntryConfig config = configs[rd];

            long attrValue = 0;

            attrValue = RandomHelper.RandomNumber(config.MinValue, config.MaxValue + 1);

            return new KeyValuePair<int, long>(config.AttrId, attrValue);
        }
    }
}
