using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{

    public class DropData
    {
        public int DropLimitId { get; set; }

        public double Number { get; set; } = 0;

        public int Seed { get; set; } = 0;


        public DropData(int dropLimitId)
        {
            this.DropLimitId = dropLimitId;
        }

        public void Init(int startSeed)
        {
            this.Seed = AppHelper.RefreshSeed(startSeed);
        }
    }
}
