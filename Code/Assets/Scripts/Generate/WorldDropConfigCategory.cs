using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class WorldDropConfigCategory
    {
        public WorldDropConfig GetConfig(int mapId, int level)
        {
            var config = this.list.Where(m => m.MapId == mapId && m.StartLevel <= level && level <= m.EndLevel).FirstOrDefault();
            return config;
        }
    }

    public partial class WorldDropConfig
    {
        public Item BuildItem(int progress)
        {

            if (progress % 100 == 0)
            {
                return ItemHelper.BuildItem((ItemType)ItemType2, ItemId2, 1, ItemCount2);
            }
            else if (progress % 10 == 0)
            {
                return ItemHelper.BuildItem((ItemType)ItemType1, ItemId1, 1, ItemCount1);
            }
            else
            {
                return ItemHelper.BuildItem((ItemType)ItemType, ItemId, 1, ItemCount);
            }
        }
    }

}
