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
            if (Current != null && Current.Count.Data <= 0)
            {
                Current = null;
            }

            if (Current == null && this.Count.Data > 0)
            {
                Current = new DefendRecord();
                Current.Progress.Data = 1;
                Current.Hp.Data = ConfigHelper.DefendHp;
                Current.Count.Data = 10;
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

        public List<DefendBuffConfig> GetBuffList(DefendBuffType type)
        {
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
