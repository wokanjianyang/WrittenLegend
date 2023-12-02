using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public class HeroPhatomData
    {

        public long Ticket { get; set; }

        public MagicData Count { get; set; } = new MagicData();

        public HeroPhatomRecord Current = null;

        private int MaxLevel = 5;

        public HeroPhatomRecord GetCurrentRecord()
        {
            if (Current != null && Current.Progress.Data > MaxLevel)
            {
                Current = null;
            }

            if (Current == null && this.Count.Data > 0)
            {
                Current = new HeroPhatomRecord();
                Current.Progress.Data = 1;

                this.Count.Data--;
            }

            return Current;
        }

        public void Refresh()
        {
            this.Current = null;
            this.Count.Data = 1;
        }

        public void Complete()
        {
            this.Current = null;
        }
    }

    public class HeroPhatomRecord
    {
        public MagicData Progress { get; set; } = new MagicData();

        public List<int> SkillIdList { get; set; } = new List<int>();

        public void Next()
        {
            this.Progress.Data++;
        }
    }
}
