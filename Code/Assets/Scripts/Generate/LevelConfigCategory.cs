using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class LevelConfigCategory
    {

        public static long GetLevelAttr(long level)
        {
            return level * (level + 1) / 2;
        }

        public static long ConvertLevel(long oldLevel,long maxLevel)
        {

            long total = 0;

            for (int i = 1; i <= maxLevel; i++)
            {
                total += i;

                if (total >= oldLevel)
                {
                    return i;
                }
            }

            return maxLevel;
        }
    }



}
