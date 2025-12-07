using Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public partial class FestiveAttrConfigCategory
    {
        public List<FestiveAttrConfig> GetList(int type, long level)
        {
            return this.list.Where(m => m.Type == type && m.StartLevel <= level).ToList();
        }


    }

}
