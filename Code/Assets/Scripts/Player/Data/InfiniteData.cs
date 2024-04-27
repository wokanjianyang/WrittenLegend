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
            //Debug.Log("nt:" + nt + "  Ticket:" + Ticket);
            if (nt > Ticket)
            {
                Ticket = nt;
                Current = new InfiniteRecord();
                Current.Progress.Data = 1;
                Current.Count.Data = 100;

                if (this.DropList.Count > 0)
                {
                    DropList.RemoveAt(0);
                }
            }

            return Current;
        }

        public int GetDropId(int level)
        {
            if (this.DropList.Count < 2)
            {
                for (int i = DropList.Count; i < 2; i++)
                {
                    var list = InfiniteDropConfigCategory.Instance.GetAllDropIdList();
                    DropList.Add(list);
                }
            }

            //Debug.Log("infinite drop1-100:" + DropList[0][99]);
            //Debug.Log("drop:" + DropList[0][99] + "," + DropList[0][199] + "," + DropList[0][299]);

            return DropList[0][level - 1];
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
    }
}
