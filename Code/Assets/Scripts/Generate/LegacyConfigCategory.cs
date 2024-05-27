using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            int result;

            int rd = RandomHelper.RandomNumber(1, 101);

            layer = Math.Min(layer, 25);

            if (rd <= 30 - layer)
            {
                result = 3;
            }
            else if (rd <= 70 - layer)
            {
                result = 2;
            }
            else if (rd <= 90 - layer)
            {
                result = 1;
            }
            else
            {
                result = 0;
            }

            return result;
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
            long layer = ConfigHelper.Max_Legacy_Level;

            for (int i = 0; i < PowerList.Length; i++)
            {
                long ml = 0;

                if (powerList[i] > PowerList[i])
                {
                    ml = 1 + (powerList[i] - PowerList[i]) / PowerRiseList[i];
                }

                layer = Math.Min(layer, ml);
            }

            return layer + 1;
        }
    }
}