using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class CompositeConfigCategory
    {

        public Dictionary<string, List<CompositeConfig>> GetList()
        {
            Dictionary<string, List<CompositeConfig>> list = new Dictionary<string, List<CompositeConfig>>();

            var groupedDictionary = GetAll().Values.GroupBy(kv => kv.Type);

            foreach (var group in groupedDictionary)
            {
                list[group.Key] = group.ToList();
            }

            return list;
        }

        public long GetTotalFee(int level)
        {
            return this.list.Where(m => m.Id >= 101 && m.Id < 100 + level).Select(m => m.ItemCountList[1]).Sum() + 2;
        }
    }

}
