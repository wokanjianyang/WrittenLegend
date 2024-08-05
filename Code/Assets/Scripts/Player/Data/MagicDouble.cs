using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class MagicDouble
    {
        private const double MagicRate = 3;

        private const double MagicOff = 10211;

        private double data;

        public double Data
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
    }
}
