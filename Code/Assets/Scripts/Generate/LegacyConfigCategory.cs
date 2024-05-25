using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class LegacyConfigCategory
    {
        public List<LegacyConfig> GetRoleList(int role)
        {
            return this.list.Where(m => m.Role == role).ToList();
        }

        public LegacyConfig GetDropItem(int role)
        {
            List<LegacyConfig> dropList = this.list.Where(m => m.Role == role).ToList();

            int total = dropList.Select(m => m.DropRate).Sum();
            int rd = RandomHelper.RandomNumber(1, total + 1);

            int endRate = 0;
            for (int i = 0; i < dropList.Count; i++)
            {
                endRate += dropList[i].DropRate;

                if (rd <= endRate)
                {
                    return dropList[i];
                }
            }

            return null;
        }

        public int GetDropLayer(int layer)
        {
            int result = layer;

            int rd = RandomHelper.RandomNumber(1, 101);

            if (rd <= 40)
            {
                result = layer;
            }
            else if (rd <= 70)
            {
                result = layer - 1;
            }
            else if (rd <= 90)
            {
                result = layer - 2;
            }
            else
            {
                result = layer - 3;
            }

            return Math.Max(1, result);
        }
    }

    public partial class LegacyConfig
    {
        public int GetRecoveryNumber(int layer)
        {
            return layer * RecoveryNubmer;
        }

        public long GetLevelAttr(int index, long level)
        {
            if (level < 1)
            {
                return 0;
            }

            return AttrValueList[index] + AttrRiseList[index] * (level - 1);
        }

        public long GetLayerAttr(int index, long level)
        {
            if (level < 1)
            {
                return 0;
            }

            return LayerValueList[index] + LayerRiseList[index] * (level - 1);
        }
    }

    public partial class LegacyMapConfig
    {
        public long CalMaxLayer(long[] powerList)
        {
            long layer = 0;

            for (int i = 1; i < PowerList.Length; i++)
            {
                if (powerList[i] > PowerList[i])
                {
                    layer = 1 + (powerList[i] - PowerList[i]) / PowerRiseList[i];
                }
            }

            return layer + 1;
        }
    }
}