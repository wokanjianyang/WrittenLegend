using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class HalidomConfigCategory
    {

        public int GetRestoreFee(long level)
        {
            long total = 0;

            for (long i = level; i > 8; i--)
            {
                total += i * 20;
            }

            return (int)total;
        }
    }




}