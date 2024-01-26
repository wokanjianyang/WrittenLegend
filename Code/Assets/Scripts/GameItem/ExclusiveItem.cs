using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

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

        [JsonIgnore]
        /// <summary>
        /// 基础属性
        /// </summary>
        public IDictionary<int, long> BaseAttrList { get; set; }


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

            BaseAttrList = new Dictionary<int, long>();
            for (int i = 0; i < ExclusiveConfig.AttrIdList.Length; i++)
            {
                long attrValue = ExclusiveConfig.AttrValueList[i] * Quality;

                BaseAttrList.Add(ExclusiveConfig.AttrIdList[i], attrValue);
            }

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

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, long> GetTotalAttrList()
        {
            int level = RuneConfigIdList.Count + 1;

            //根据基础属性和词条属性，计算总属性
            IDictionary<int, long> AttrList = new Dictionary<int, long>();
            foreach (int attrId in BaseAttrList.Keys)
            {
                AttrList[attrId] = BaseAttrList[attrId] * level;
            }

            return AttrList;
        }

        public void Devour(ExclusiveItem exclusive)
        {
            this.RuneConfigIdList.Add(exclusive.RuneConfigId);
            this.SuitConfigIdList.Add(exclusive.SuitConfigId);

            this.Level = this.RuneConfigIdList.Count;
        }
    }
}
