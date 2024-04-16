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

        public List<List<int>> DropList = new List<List<int>>();

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

        public int GetDropId(int level)
        {
            if (this.DropList.Count < 2)
            {
                for (int i = DropList.Count; i < 2; i++)
                {
                    DropList.Add(InfiniteDropConfigCategory.Instance.GetAllDropIdList());
                }
            }

            Debug.Log("infinite drop1:" + DropList[0].ListToString());
            Debug.Log("infinite drop2:" + DropList[1].ListToString());

            return DropList[0][level - 1];
        }


        public void Complete()
        {
            this.Current = null;
            DropList.RemoveAt(0);
        }
    }

    public class InfiniteRecord
    {
        public MagicData Progress { get; set; } = new MagicData();

        public MagicData Count { get; set; } = new MagicData();
    }
}
