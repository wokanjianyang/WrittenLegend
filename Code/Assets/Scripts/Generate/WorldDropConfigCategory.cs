using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class WorldDropConfigCategory
    {
        public Item BuildItem(int mapId, int progress)
        {
            if (progress > 0 && progress % 80 == 0)
            {

                List<WorldDropConfig> dropList = this.list.Where(m => m.MapId == mapId && m.DropType == 2).ToList();

                int id = (progress / 100 - 1) % dropList.Count;
                return ItemHelper.BuildItem(ItemType.Material, dropList[id].ItemId, 1, 1);
            }
            else
            {
                List<WorldDropConfig> dropList = this.list.Where(m => m.MapId == mapId && m.DropType == 1).ToList();

                int id = (progress - 1) % dropList.Count;

                return ItemHelper.BuildItem(ItemType.Material, dropList[id].ItemId, 1, 1);
            }
        }
    }
}
