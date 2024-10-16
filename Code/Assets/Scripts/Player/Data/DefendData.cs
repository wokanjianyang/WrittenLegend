﻿using Game.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

    public class DefendData
    {

        public long Ticket { get; set; }

        public Dictionary<int, MagicData> CountDict { get; set; } = new Dictionary<int, MagicData>();

        public Dictionary<int, DefendRecord> CurrentDict = new Dictionary<int, DefendRecord>();

        public Dictionary<int, List<List<int>>> DropDict = new Dictionary<int, List<List<int>>>();

        public DefendRecord GetCurrentRecord(int level)
        {
            CurrentDict.TryGetValue(level, out DefendRecord Current);
            return Current;
        }

        public void BuildCurrent()
        {
            for (int level = 1; level <= ConfigHelper.DefendMaxLevel; level++)
            {
                CurrentDict.TryGetValue(level, out DefendRecord Current);

                if (Current != null && Current.Count.Data <= 0)
                {
                    Current = null;
                    CurrentDict.Remove(level);
                }

                if (!CountDict.ContainsKey(level))
                {
                    MagicData data = new MagicData();
                    data.Data = 1;
                    CountDict[level] = data;
                }

                CountDict.TryGetValue(level, out MagicData Count);
                if (Current == null && Count.Data > 0)
                {
                    Current = new DefendRecord();
                    Current.Progress.Data = 1;
                    Current.Hp.Data = ConfigHelper.DefendHp;
                    Current.Count.Data = 10;
                    CurrentDict[level] = Current;

                    Count.Data--;

                    if (DropDict.TryGetValue(level, out List<List<int>> dropList))
                    {
                        if (dropList.Count > 0)
                        {
                            dropList.RemoveAt(0);
                        }
                    }
                }

            }
        }

        public void Refresh()
        {
            CurrentDict.Clear();

            foreach (var Count in CountDict)
            {
                Count.Value.Data = 1;
            }
        }

        public void Complete()
        {
            this.CurrentDict.Remove(AppHelper.DefendLevel);
        }

        public int GetDropId(int layer, int progress)
        {
            return GetDropIdList(layer)[progress - 1];
        }

        public List<int> GetDropIdList(int layer)
        {
            if (!DropDict.ContainsKey(layer))
            {
                DropDict[layer] = new List<List<int>>();
            }

            List<List<int>> DropList = DropDict[layer];

            if (DropList.Count < 2)
            {
                for (int i = DropList.Count; i < 2; i++)
                {
                    var list = DefendDropConfigCategory.Instance.GetAllDropIdList(layer);
                    DropList.Add(list);
                }
            }

            //Debug.Log("infinite drop1-100:" + DropList[0][99]);
            //Debug.Log("drop:" + DropList[0][99] + "," + DropList[0][199] + "," + DropList[0][299]);

            return DropList[0];
        }

        public List<DefendBuffConfig> GetBuffList(DefendBuffType type)
        {
            int level = AppHelper.DefendLevel;
            CurrentDict.TryGetValue(level, out DefendRecord Current);

            List<DefendBuffConfig> list = new List<DefendBuffConfig>();

            if (Current != null)
            {
                foreach (var kv in Current.BuffDict)
                {
                    DefendBuffConfig config = DefendBuffConfigCategory.Instance.Get(kv.Value);
                    if (config.Type == (int)type)
                    {
                        list.Add(config);
                    }
                }
            }
            return list;
        }

        public List<int> GetExcludeList()
        {
            int level = AppHelper.DefendLevel;
            CurrentDict.TryGetValue(level, out DefendRecord Current);

            List<int> list = new List<int>();

            if (Current != null)
            {
                var gp = Current.BuffDict.Select(m => m.Value).GroupBy(m => m);
                foreach (var g in gp)
                {
                    int buffId = g.Key;
                    int count = g.Count();
                    DefendBuffConfig config = DefendBuffConfigCategory.Instance.Get(buffId);

                    if (config.MaxCount <= count)
                    {
                        list.Add(config.Id);

                        //Debug.Log("Exclued buff:" + config.Name + " count:" + count);
                    }
                }
            }
            return list;
        }

        public List<SkillRuneConfig> GetBuffRuneList(int skillId)
        {
            List<SkillRuneConfig> list = new List<SkillRuneConfig>();

            List<DefendBuffConfig> configs = GetBuffList(DefendBuffType.Rune);

            foreach (DefendBuffConfig config in configs)
            {
                SkillRuneConfig runeConfig = SkillRuneConfigCategory.Instance.Get(config.RuneId);
                if (runeConfig.SkillId == skillId)
                {
                    list.Add(runeConfig);
                }
            }
            return list;
        }
    }

    public class DefendRecord
    {
        public MagicData Progress { get; set; } = new MagicData();

        public MagicData Hp { get; set; } = new MagicData();

        public MagicData Count { get; set; } = new MagicData();

        public Dictionary<int, int> BuffDict = new Dictionary<int, int>();
    }
}
