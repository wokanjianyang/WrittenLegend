using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class WorldData
    {

        public Dictionary<int, int> Record { get; set; } = new Dictionary<int, int>();

        public long Ticket { get; set; }


        public void Check()
        {
            DateTime today = DateTime.Today;
            int difference = ((int)DayOfWeek.Monday - (int)today.DayOfWeek);
            DateTime currentMonday = today.AddDays(difference);

            long nt = currentMonday.Ticks;

            if (Ticket == 0 || nt > Ticket)
            {
                Ticket = nt;

                Record = new Dictionary<int, int>();
            }
        }

        public int GetLayer(int id)
        {
            if (!Record.ContainsKey(id))
            {
                Record[id] = 0;
            }

            return Record[id];
        }

        public void SetOver(int id)
        {
            this.Ticket = DateTime.Now.Ticks;
        }
    }
}
