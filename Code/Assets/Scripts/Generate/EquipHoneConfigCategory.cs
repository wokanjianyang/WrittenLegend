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

            int replayLevel = (int)Math.Ceiling((config.StartValue - (double)attrVal) / config.AttrValue);

            return replayLevel + layer;
        }

        public int GetNeedNumber(int honeLevel)
        {
            return honeLevel + 5;
        }

        public int GetTotalNeedNumber(int honeLevel)
        {
            int total = 0;
            for (int i = 0; i < honeLevel; i++)
            {
                total += GetNeedNumber(i);
            }
            return total;
        }
    }


}