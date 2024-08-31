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


        public void Check(long cycle)
        {
            if (Time == null)
            {
                Time = new MagicDouble();
            }

            long nt = DateTime.Today.Ticks;

            if (Ticket == 0 || nt > Ticket)
            {
                Ticket = nt;
                if (cycle <= 0)
                {
                    Time.Data = 0;
                }

                if (Time.Data < 6000 && cycle > 0)
                {
                    Time.Data += ConfigHelper.PillDefaultTime * 10;
                }
            }
        }
    }
}
