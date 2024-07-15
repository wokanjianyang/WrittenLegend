using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class CardConfigCategory
    {

    }

    public partial class CardConfig
    {

        public long CalUpLevel(long currentLevel, long materialNubmer, out long useNumber)
        {
            useNumber = 0;
            long upLevel = 0;

            while (materialNubmer > 0)
            {
                long tempUpNumber = CalNewUpNumber(currentLevel);

                if (tempUpNumber <= materialNubmer)
                {
                    upLevel++;
                    currentLevel++;
                    useNumber += tempUpNumber;
                }
                materialNubmer -= tempUpNumber;
            }

            return upLevel;
        }

        public long CalNewUpNumber(long currentLevel)
        {
            long rise = Math.Min(currentLevel / RiseLevel, 10);
            rise = rise * RiseNumber + StartNubmer;
            return rise;

        }

        public long CalOldUpNumber(long currentLevel)
        {
            long rise = currentLevel / RiseLevel;
            rise = rise * RiseNumber + StartNubmer;
            return rise;
        }

        public long CalReturnNumber(long currentLevel)
        {
            long newTotal = 0;
            long oldTotal = 0;

            for (int i = 0; i < currentLevel; i++)
            {
                newTotal += CalNewUpNumber(i);
                oldTotal += CalOldUpNumber(i);
            }

            return oldTotal - newTotal;
        }
    }

}
