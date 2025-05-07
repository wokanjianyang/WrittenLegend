using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class FestiveWeekData
    {

        public Dictionary<int, int> Record { get; set; } = new Dictionary<int, int>();

        public long Ticket { get; set; }

        public bool Check()
        {
            long week = TimeHelper.GetFestiveWeek();

            if (week > Ticket)
            {
                Ticket = week;
                Record.Clear();
                return true;
            }

            return false;
        }

        public int GetFestiveWeekCount(int id)
        {
            if (!this.Record.ContainsKey(id))
            {
                this.Record[id] = 0;
            }

            return this.Record[id];
        }

        public void SaveFestiveWeekCount(int configId, int count)
        {
            if (this.Record.ContainsKey(configId))
            {
                this.Record[configId] += count;
            }
            else
            {
                this.Record[configId] = count;
            }
        }
    }
}
