using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class MythData
    {

        public Dictionary<int, double> Record { get; set; } = new Dictionary<int, double>();

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

                Record = new Dictionary<int, double>();
            }
        }

        public bool GetOver(int id)
        {
            if (!Record.ContainsKey(id))
            {
                Record[id] = 0;
            }

            if (Record[id] > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetMax()
        {
            var seq = Record.Where(m => m.Value > 0).Select(m => m.Key);
            if (seq.Any())
            {
                return seq.Max();
            }
            else
            {
                return 0;
            }
        }

        public void SetOver(int id)
        {
            this.Record[id] = DateTime.Now.Ticks;
        }
    }
}
