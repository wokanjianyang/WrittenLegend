using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class DropData
    {
        public int DropLimitId { get; set; }

        public int Number { get; set; } = 0;

        public int Seed { get; set; } = 0;


        public DropData(int dropLimitId)
        {
            this.DropLimitId = dropLimitId;
        }

        public void Init(int startSeed)
        {
            this.Seed = RandomHelper.RandomNumber(startSeed, 0, int.MaxValue - 100000000);
        }
    }
}
