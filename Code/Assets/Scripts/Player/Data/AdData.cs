﻿using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class AdData
    {
        //public MagicData RealCount { get; set; } = new MagicData();

        //public MagicData VirtualCount { get; set; } = new MagicData();

        private static int Total = 360;

        Dictionary<string, AdRecord> CodeDict { get; } = new Dictionary<string, AdRecord>();

        public void SaveCode(string code)
        {
            AdRecord data = new AdRecord();
            //data.Total.Data = 360;
            data.Count = new MagicData();
            data.Count.Data = 0;
            data.Code = code;
            data.Check = true;

            this.CodeDict[code] = data;
        }

        public int GetSkipCount()
        {
            return (int)(CodeDict.Select(m => m.Value).Where(m => m.Check && m.Count.Data < Total)
                .Select(m => Total - m.Count.Data).Sum());
        }

        public void Use()
        {
            AdRecord record = CodeDict.Select(m => m.Value).Where(m => m.Check && m.Count.Data < Total).FirstOrDefault();
            if (record != null)
            {
                record.Count.Data++;
                //this.RealCount.Data++;
            }
        }

        public void Check()
        {
            List<AdRecord> records = CodeDict.Select(m => m.Value).Where(m => m.Count.Data < Total).ToList();

            foreach (AdRecord record in records)
            {
                CodeConfig config = CodeConfigCategory.Instance.GetSpeicalConfig(record.Code);
                if (config != null)
                {
                    record.Check = true;
                }
                else
                {
                    record.Check = false;
                }
            }
        }

        //public void AddReal()
        //{
        //    this.RealCount.Data++;
        //}

        //public void AddVirtual()
        //{
        //    this.VirtualCount.Data++;
        //}
    }

    public class AdRecord
    {
        public string Code { get; set; }

        public MagicData Count { get; set; }
        //public MagicData Total { get; set; }

        public bool Check = false;
    }
}
