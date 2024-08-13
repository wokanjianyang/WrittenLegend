using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class BabelConfigCategory
    {
        public BabelConfig GetByProgress(long progress)
        {
            var config = this.list.Where(m => m.Start <= progress && progress <= m.End).FirstOrDefault();
            return config;
        }
    }

    public partial class BabelConfig
    {

        public int GetItemId(long progress)
        {
            if (progress % 100 == 0)
            {
                return this.ItemId2;
            }
            else if (progress % 10 == 0)
            {
                return this.ItemId1;
            }
            else
            {
                return this.ItemId;
            }
        }

        public int GetItemCount(long progress)
        {
            if (progress % 100 == 0)
            {
                return this.ItemCount2;
            }
            else if (progress % 10 == 0)
            {
                return this.ItemCount1;
            }
            else
            {
                return this.ItemCount;
            }
        }

        public Item BuildItem(long progress)
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
