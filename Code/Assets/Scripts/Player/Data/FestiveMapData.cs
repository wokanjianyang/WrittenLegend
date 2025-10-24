using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class FestiveMapData
    {

        public int Record { get; set; }

        public long Ticket { get; set; }

        public MagicData Number { get; set; } = new MagicData();


        public void Check()
        {
            DateTime today = DateTime.Today;
            long nt = today.Ticks;

            if (Ticket == 0 || nt > Ticket)
            {
                Ticket = nt;

                Number.Data = 10;
            }
        }
    }
}
