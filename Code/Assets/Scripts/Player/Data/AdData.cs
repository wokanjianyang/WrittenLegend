using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class AdData
    {
        public MagicData RealCount { get; set; } = new MagicData();

        public MagicData VirtualCount { get; set; } = new MagicData();

        Dictionary<string, AdRecord> CodeDict { get; } = new Dictionary<string, AdRecord>();

        public void SaveCode(string code)
        {
            AdRecord data = new AdRecord();
            data.Total.Data = 360;
            data.Count.Data = 0;
            data.Code = code;

            this.CodeDict[code] = data;
        }

        public int GetSkipCount()
        {
            return (int)CodeDict.Select(m => m.Value.Total.Data - m.Value.Count.Data).Sum();
        }

        public void Use()
        {
            AdRecord record = CodeDict.Select(m => m.Value).Where(m => m.Count.Data < m.Total.Data).FirstOrDefault();
            if (record != null)
            {
                record.Count.Data++;
                this.RealCount.Data++;
            }
        }

        public void AddReal()
        {
            this.RealCount.Data++;
        }

        public void AddVirtual()
        {
            this.VirtualCount.Data++;
        }
    }

    public class AdRecord
    {
        public string Code { get; set; }

        public MagicData Count { get; set; }
        public MagicData Total { get; set; }
    }
}
