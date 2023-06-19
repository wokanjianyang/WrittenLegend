using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class SynthesisConfigCategory
    {

        public Dictionary<string, List<SynthesisConfig>> GetList()
        {
            Dictionary<string, List<SynthesisConfig>> list = new Dictionary<string, List<SynthesisConfig>>();

            var groupedDictionary = GetAll().Values.GroupBy(kv => kv.Type);

            foreach (var group in groupedDictionary)
            {
                list[group.Key] = group.ToList();
            }

            return list;
        }
    }

}
