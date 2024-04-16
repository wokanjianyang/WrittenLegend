using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ArtifactConfigCategory
    {
        public ArtifactConfig GetByItemId(int itemId)
        {
            return this.list.Where(m => m.ItemId == itemId).FirstOrDefault();
        }
    }

}
