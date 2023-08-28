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

        private string text;

        public long Data
        {
            get
            {
                if (data <= 1)
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
                if (value <= 1)
                {
                    data = value;
                }
                else
                {
                    data = value * MagicRate + MagicOff;
                }
            }
        }

        public bool Check()
        {
            return true;
        }
    }


}
