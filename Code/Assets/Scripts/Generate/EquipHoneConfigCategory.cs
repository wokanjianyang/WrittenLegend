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

        public int GetMaxLevel(int attrId, long attrVal, int layer)
        {
            EquipHoneConfig config = GetByAttrId(attrId);

            int replayLevel = (config.StartValue - (int)attrVal) / config.AttrValue;

            return replayLevel + layer;
        }
    }


}