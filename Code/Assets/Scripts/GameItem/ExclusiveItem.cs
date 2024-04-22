using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;

namespace Game
{
    public class ExclusiveItem : Item
    {
        public int RuneConfigId { get; set; }

        public int SuitConfigId { get; set; }

        public int Quality { get; set; }

        public int DoubleHitId { get; set; }

        public List<int> RuneConfigIdList { get; set; } = new List<int>();

        public List<int> SuitConfigIdList { get; set; } = new List<int>();

        public override int GetQuality()
        {
            return Quality;
        }

        [JsonIgnore]
        public SkillRuneConfig SkillRuneConfig { get; set; }

        [JsonIgnore]
        public SkillSuitConfig SkillSuitConfig { get; set; }

        [JsonIgnore]
        public ExclusiveConfig ExclusiveConfig { get; set; }

        [JsonIgnore]
        public SkillDoubleHitConfig DoubleHitConfig { get; set; }

        [JsonIgnore]
        public int Part { get; set; }

        public ExclusiveItem(int configId, int runeConfigId, int suitConfigId, int quality, int doubleHitId)
        {
            this.Type = ItemType.Exclusive;
            this.ConfigId = configId;
            this.RuneConfigId = runeConfigId;
            this.SuitConfigId = suitConfigId;
            this.DoubleHitId = doubleHitId;

            ExclusiveConfig = ExclusiveConfigCategory.Instance.Get(configId);

            Name = ExclusiveConfig.Name;
            Des = ExclusiveConfig.Name;
            Level = 0;
            Part = ExclusiveConfig.Part;
            Gold = 0;
            Quality = quality;

            if (RuneConfigId > 0)
            {
                SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(RuneConfigId);
            }

            if (SuitConfigId > 0)
            {
                SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(SuitConfigId);
            }

            if (DoubleHitId > 0)
            {
                DoubleHitConfig = SkillDoubleHitConfigCategory.Instance.Get(DoubleHitId);
            }
        }

        public void Init(int seed)
        {
            this.Seed = seed;
        }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, long> GetTotalAttrList()
        {
            return this.GetBaseAttrList();
        }

        public IDictionary<int, long> GetBaseAttrList()
        {
            int level = GetLevel();

            IDictionary<int, long> BaseAttrList = new Dictionary<int, long>();

            ExclusiveAttrConfig attrConfig = ExclusiveAttrConfigCategory.Instance.GetByLevel(level);
            for (int i = 0; i < attrConfig.AttrIdList.Length; i++)
            {
                BaseAttrList.Add(attrConfig.AttrIdList[i], attrConfig.AttrValueList[i] * Quality);
            }

            return BaseAttrList;
        }

        public List<SkillRuneConfig> GetRuneList(int skillId)
        {
            List<SkillRuneConfig> list = new List<SkillRuneConfig>();

            if (SkillRuneConfig != null && SkillRuneConfig.SkillId == skillId)
            {
                list.Add(SkillRuneConfig);
            }

            for (int i = 0; i < RuneConfigIdList.Count; i++)
            {
                SkillRuneConfig config = SkillRuneConfigCategory.Instance.Get(RuneConfigIdList[i]);
                if (config.SkillId == skillId)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        public List<SkillSuitConfig> GetSuitList(int skillId)
        {
            List<SkillSuitConfig> list = new List<SkillSuitConfig>();

            if (SkillSuitConfig != null && SkillSuitConfig.SkillId == skillId)
            {
                list.Add(SkillSuitConfig);
            }

            for (int i = 0; i < SuitConfigIdList.Count; i++)
            {
                SkillSuitConfig config = SkillSuitConfigCategory.Instance.Get(SuitConfigIdList[i]);
                if (config.SkillId == skillId)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        public int GetSuitCount(int suitId)
        {
            return SuitConfigIdList.Where(m => m == suitId).Count() + (SuitConfigId == suitId ? 1 : 0);
        }

        public int GetLevel()
        {
            return RuneConfigIdList.Count + 1;
        }

        public void Devour(ExclusiveItem exclusive)
        {
            this.RuneConfigIdList.Add(exclusive.RuneConfigId);
            this.SuitConfigIdList.Add(exclusive.SuitConfigId);
        }
    }
}
