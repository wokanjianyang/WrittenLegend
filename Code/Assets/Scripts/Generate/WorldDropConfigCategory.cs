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

        private int[] Drop1_List1 = new int[]{
                  60000003, 60000002, 60000006, 60000007, 60000008, 60000009, 60000001, 60000003, 60000002, 60000006
                , 60000007, 60000008, 60000009, 60000001, 60000010, 60000003, 60000002, 60000006, 60000007, 60000008
                , 60000009, 60000001, 60000003, 60000002, 60000006, 60000007, 60000008, 60000009, 60000001, 60000010
                , 60000005, 60000003, 60000002, 60000006, 60000007, 60000008, 60000009, 60000001, 60000003, 60000002
                , 60000006, 60000007, 60000008, 60000009, 60000001, 60000010, 60000005, 60000004, 60000003, 60000002
        };

        private int[] Drop1_List2 = new int[] { 61000001, 61000002, 61000003, 61000004, 61000005, 61000006, 61000007, 61000008 };

        public Item BuildItem(int mapId, int progress)
        {

            if (progress % 100 != 0)
            {
                int id = progress % Drop1_List1.Length;

                return ItemHelper.BuildItem(ItemType.Material, Drop1_List1[id], 1, 1);
            }
            else
            {
                int id = (progress / 100) % Drop1_List2.Length;
                return ItemHelper.BuildItem(ItemType.Material, Drop1_List2[id], 1, 1);
            }
        }
    }
}
