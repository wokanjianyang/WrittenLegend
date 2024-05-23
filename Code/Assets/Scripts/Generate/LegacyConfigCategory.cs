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

            if (rd <= 10)
            {
                result = layer;
            }
            else if (rd <= 30)
            {
                result = layer - 1;
            }
            else if (rd <= 60)
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
        public int GetRecoveryNumber(long layer)
        {
            return (int)Math.Pow(2, layer - 1) * RecoveryNubmer;
        }
    }

    public partial class LegacyMapConfig
    {
        public long CalMaxLayer(long[] powerList)
        {
            long layer = powerList[0] / PowerList[0];

            for (int i = 1; i < PowerList.Length; i++)
            {
                layer = Math.Min(layer, powerList[i] / PowerList[i]);
            }

            return layer + 1;
        }
    }
}