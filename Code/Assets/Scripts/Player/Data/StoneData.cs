using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class StoneSet
    {
        public int StoneId { get; set; } = 0;

        public MagicData StoneLevel { get; set; }

        public StoneSet(int stoneId)
        {
            this.StoneId = stoneId;
            this.StoneLevel = new MagicData();
        }
    }

    public class StoneRecord
    {
        public Dictionary<int, StoneSet> List { get; set; } = new Dictionary<int, StoneSet>();

        public int SetCount = 0;

        public int GetSetCount()
        {
            return SetCount;
        }

        public int GetStoneId(int index)
        {
            if (!List.ContainsKey(index))
            {
                return 0;
            }

            return List[index].StoneId;
        }

        public List<int> GetExcludeStoneId(int index)
        {
            return List.Where(m => m.Key != index).Select(m => m.Value.StoneId).ToList();
        }

        public int GetStoneLevel(int index)
        {
            if (!List.ContainsKey(index))
            {
                return 0;
            }

            return (int)List[index].StoneLevel.Data;
        }

        public void AddCount()
        {
            this.SetCount++;
        }

        public void AddLevel(int index, int stoneId)
        {
            if (!List.ContainsKey(index))
            {
                if (List.Count >= SetCount + 1)
                {
                    Debug.Log("没有镶嵌位置");
                    return; //如果没有镶嵌位置，不可以镶嵌
                }

                List[index] = new StoneSet(stoneId);
            }

            List[index].StoneLevel.Data++;
        }

        public long GetTotalLevel()
        {
            return this.List.Select(m => m.Value.StoneLevel.Data).Sum();
        }
    }
}
