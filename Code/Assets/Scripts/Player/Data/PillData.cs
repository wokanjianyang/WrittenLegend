using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class PillTime
    {

        public long Ticket { get; set; }

        public MagicDouble Time { get; set; }


        public void Check()
        {
            long nt = DateTime.Today.Ticks;

            if (Ticket == 0 || nt > Ticket)
            {
                Ticket = nt;
                Time.Data += 20 * 60;
            }
        }
    }
}
