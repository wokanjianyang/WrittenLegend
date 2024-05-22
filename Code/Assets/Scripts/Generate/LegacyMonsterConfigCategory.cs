using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class LegacyMonsterConfigCategory
    {
        public LegacyMonsterConfig GetByRole(int role)
        {
            return this.list.Where(m => m.Role == role).FirstOrDefault();
        }
    }

}