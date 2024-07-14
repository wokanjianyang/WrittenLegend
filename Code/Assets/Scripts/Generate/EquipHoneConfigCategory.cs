using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class EquipHoneConfigCategory
    {
        public EquipHoneConfig GetByAttrId(int attrId)
        {
            EquipHoneConfig config = this.list.Where(m => m.AttrId == attrId).FirstOrDefault();
            return config;
        }
    }
}