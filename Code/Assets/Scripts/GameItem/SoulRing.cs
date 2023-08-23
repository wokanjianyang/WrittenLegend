using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class SoulRing 
    {
        public List<KeyValuePair<int, long>> AttrEntryList { get; set; } = new List<KeyValuePair<int, long>>();

        public int RuneConfigId { get; set; }

        public int SuitConfigId { get; set; }

        public int Quality { get; set; }


        public SoulRing(int configId, int level)
        {
           
        }

        public IDictionary<int, long> GetTotalAttrList()
        {
            IDictionary<int, long> AttrList = new Dictionary<int, long>();

            return AttrList;
        }
    }
}
