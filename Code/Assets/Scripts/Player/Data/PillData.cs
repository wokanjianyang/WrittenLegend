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
            if (Time == null)
            {
                Time = new MagicDouble();
            }

            long nt = DateTime.Today.Ticks;

            if (Ticket == 0 || nt > Ticket)
            {
                Ticket = nt;
                if (Time.Data < 6000)
                {
                    Time.Data += ConfigHelper.PillDefaultTime * 10;
                }
            }
        }
    }
}
