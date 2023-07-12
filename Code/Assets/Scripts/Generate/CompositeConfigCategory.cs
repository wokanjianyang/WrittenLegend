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
    }

}
