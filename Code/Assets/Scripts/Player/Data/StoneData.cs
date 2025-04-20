using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class StoneRecord
    {

        public Dictionary<int, MagicData> LeveList { get; set; } = new Dictionary<int, MagicData>();
        public Dictionary<int, int> IdList { get; set; } = new Dictionary<int, int>();

        public int RunCount = 0;

        public int GetSetCount()
        {
            return RunCount;
        }

        public int GetStoneId(int index)
        {
            if (!IdList.ContainsKey(index))
            {
                IdList[index] = 0;
            }

            return IdList[index];
        }

        public int GetStoneLevel(int id)
        {
            if (id == 0)
            {
                return 0;
            }

            if (!LeveList.ContainsKey(id))
            {
                LeveList[id] = new MagicData();
            }

            return (int)LeveList[id].Data;
        }

        public void AddCount()
        {
            this.RunCount++;
        }

        public void AddLevel(int id)
        {
            LeveList[id].Data++;
        }
    }
}
