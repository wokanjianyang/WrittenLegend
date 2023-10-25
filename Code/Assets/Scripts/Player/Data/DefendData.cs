using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public class DefendData
    {

        public long Ticket { get; set; }

        public MagicData Count { get; set; } = new MagicData();

        public DefendRecord Current = null;

        public DefendRecord GetCurrentRecord()
        {
            if (Current == null && this.Count.Data >= 0)
            {
                Current = new DefendRecord();
                Current.Progress.Data = 1;
                Current.Hp.Data = ConfigHelper.DefendHp;
                //this.Count.Data--;
            }

            return Current;
        }

        public void Refresh()
        {
            this.Count.Data = 1;
        }

        public void Complete()
        {
            this.Current = null;
        }
    }
    public class DefendRecord
    {
        public MagicData Progress { get; set; } = new MagicData();

        public MagicData Hp { get; set; } = new MagicData();
    }
}
