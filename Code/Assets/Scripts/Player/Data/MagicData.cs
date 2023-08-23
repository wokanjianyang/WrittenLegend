using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class MagicData
    {
        private const long MagicRate = 3;

        private const long MagicOff = 10211;

        private long data;

        public long Data
        {
            get
            {
                if (data == 0)
                {
                    return data;
                }
                else
                {
                    return (data - MagicOff) / MagicRate;
                }
            }
            set
            {
                data = value * MagicRate + MagicOff;
            }
        }
    }


}
