using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class BabelMythConfigCategory
    {
        public BabelMythConfig GetByProgress(long progress)
        {
            var config = this.list.Where(m => m.Start <= progress && progress <= m.End).FirstOrDefault();
            return config;
        }
    }

    public partial class BabelMythConfig
    {
        public Item BuildItem(long progress)
        {

            for (int i = this.ItemIdList.Length - 1; i >= 0; i--)
            {
                int rate = ItemRateList[i];
                if (progress % rate == 0)
                {
                    return ItemHelper.BuildItem((ItemType)ItemTypeList[i], ItemIdList[i], 1, ItemCountList[i]);
                }
            }


            return ItemHelper.BuildItem((ItemType)ItemTypeList[0], ItemIdList[0], 1, ItemCountList[0]);
        }

    }
}
