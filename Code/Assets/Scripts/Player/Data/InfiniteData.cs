using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class InfiniteData
    {

        public long Ticket { get; set; }

        public Dictionary<int, MagicData> CountDict { get; set; } = new Dictionary<int, MagicData>();

        public InfiniteRecord Current { get; set; }

        public InfiniteRecord GetCurrentRecord()
        {
            long nt = DateTime.Today.Ticks;
            if (nt > Ticket)
            {
                Ticket = nt;
                Current = new InfiniteRecord();
                Current.Progress.Data = 1;
                Current.Count.Data = 10;
            }

            return Current;
        }

        public void BuildCurrent()
        {

        }

        public void Complete()
        {
            this.Current = null;
        }
    }

    public class InfiniteRecord
    {
        public MagicData Progress { get; set; } = new MagicData();

        public MagicData Count { get; set; } = new MagicData();

        public Dictionary<int, int> DropDict = new Dictionary<int, int>();
    }
}
