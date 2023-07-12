using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Equip : Item
    {
        /// <summary>
        /// 词条属性列表
        /// </summary>
        public List<KeyValuePair<int, long>> AttrEntryList { get; set; } = new List<KeyValuePair<int, long>>();

        public int RuneConfigId { get; set; }

        public int SuitConfigId { get; set; }

        public int Quality { get; set; }

        public Equip(int configId, int runeConfigId, int suitConfigId)
        {
            this.Type = ItemType.Equip;
            this.ConfigId = configId;
            this.RuneConfigId = runeConfigId;
            this.SuitConfigId = suitConfigId;

            EquipConfig = EquipConfigCategory.Instance.Get(configId);

            Name = EquipConfig.Name;
            Des = EquipConfig.Name;
            Level = EquipConfig.LevelRequired;
            Part = EquipConfig.Part;
            Position = EquipConfig.Position;
            Gold = EquipConfig.Price;

            BaseAttrList = new Dictionary<int, long>();
            for (int i = 0; i < EquipConfig.AttributeBase.Length; i++)
            {
                BaseAttrList.Add(EquipConfig.BaseArray[i], EquipConfig.AttributeBase[i]);
            }

            if (RuneConfigId > 0)
            {
                SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(RuneConfigId);
            }

            if (SuitConfigId > 0)
            {
                SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(SuitConfigId);
            }
        }

        public override int GetQuality() {
            return Quality;
        }

        [JsonIgnore]
        public SkillRuneConfig SkillRuneConfig { get; set; }

        [JsonIgnore]
        public SkillSuitConfig SkillSuitConfig { get; set; }

        [JsonIgnore]
        public EquipConfig EquipConfig { get; set; }

        [JsonIgnore]
        public int[] Position { get; set; }

        [JsonIgnore]
        public int Part { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 基础属性
        /// </summary>
        public IDictionary<int, long> BaseAttrList { get; set; }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, long> GetTotalAttrList(EquipRefineConfig refineConfig)
        {
            long basePercent = 100;
            long qualityPercent = 100;

            if (refineConfig != null) {
                basePercent += refineConfig.BaseAttrPercent;
                qualityPercent += refineConfig.QualityAttrPercent;
            }

            //根据基础属性和词条属性，计算总属性
            IDictionary<int, long> AttrList = new Dictionary<int, long>();
            foreach (int attrId in BaseAttrList.Keys)
            {
                AttrList[attrId] = BaseAttrList[attrId] * basePercent / 100;
            }

            for (int i = 0; i < AttrEntryList.Count; i++)
            {
                int attrId = AttrEntryList[i].Key;
                long attrValue = AttrEntryList[i].Value * qualityPercent / 100;

                AttrList.TryGetValue(attrId, out long val);
                AttrList[attrId] = val + attrValue;
            }

            return AttrList;
        }
    }
}
