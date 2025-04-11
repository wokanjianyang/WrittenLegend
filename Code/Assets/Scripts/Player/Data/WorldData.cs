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
            long nt = TimeHelper.ClientNowSeconds();

            if (Ticket == 0 || nt - Ticket >= 86400 * 10)
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

            return Record[id] + 1;
        }

        public void SetOver(int id)
        {
            this.Record[id]++;
        }
    }
}
