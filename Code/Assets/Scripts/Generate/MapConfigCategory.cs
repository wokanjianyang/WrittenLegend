using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class MapConfigCategory
    {
        public int GetMinMapId()
        {
            return this.list.Select(m => m.Id).Min();
        }
        public int GetMaxMapId()
        {
            return this.list.Select(m => m.Id).Max();
        }
    }
}