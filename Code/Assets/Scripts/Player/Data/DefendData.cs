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

        public Dictionary<int, MagicData> CountDict { get; set; } = new Dictionary<int, MagicData>();

        public Dictionary<int, DefendRecord> CurrentDict = new Dictionary<int, DefendRecord>();

        public DefendRecord GetCurrentRecord()
        {
            int level = AppHelper.DefendLevel;
            CurrentDict.TryGetValue(level, out DefendRecord Current);

            if (Current != null && Current.Count.Data <= 0)
            {
                Current = null;
                CurrentDict.Remove(level);
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
            }

            return Current;
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
                foreach (var kv in Current.BuffDict)
                {
                    DefendBuffConfig config = DefendBuffConfigCategory.Instance.Get(kv.Value);
                    if (config.Type != (int)DefendBuffType.Attr)
                    {
                        list.Add(config.Id);
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
