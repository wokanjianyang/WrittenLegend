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
        

        public Equip(int configId, int runeConfigId, int suitConfigId,int quality)
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
            Quality = quality;

            BaseAttrList = new Dictionary<int, long>();
            for (int i = 0; i < EquipConfig.AttributeBase.Length; i++)
            {
                long AttributeBase = EquipConfig.AttributeBase[i];
                if (EquipConfig.Quality == 0)  //随机品质装备 40%,60%,80%,100%
                {
                    if (Quality <= 4)
                    {
                        AttributeBase = AttributeBase * (Quality * 20 + 20) / 100;
                    }
                    else
                    {
                        AttributeBase = AttributeBase * 2;
                    }
                }
                BaseAttrList.Add(EquipConfig.BaseArray[i], AttributeBase);
            }

            QualityAttrList = new Dictionary<int, long>();
            if (Quality > 0 && Part <= 10)
            {
                EquipQualityConfig qualityConfig = EquipQualityConfigCategory.Instance.Get(quality);

                if (qualityConfig.AttrIdList != null)
                {
                    for (int i = 0; i < qualityConfig.AttrIdList.Length; i++)
                    {
                        QualityAttrList.Add(qualityConfig.AttrIdList[i], qualityConfig.AttrValueList[i]);
                    }
                }
            }

            //根据品质,生成随机属性
            if (EquipConfig.RandomAttr == 0 && Part <= 10)
            {
                for (int i = 0; i < Quality; i++)
                {
                    var ra = AttrEntryConfigCategory.Instance.Build(this.Part, this.Level);
                    if (ra.Key > 0)
                    {
                        this.AttrEntryList.Add(ra);
                    }
                }
            }

            if (RuneConfigId > 0 && Part <= 10)
            {
                SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(RuneConfigId);
            }

            if (SuitConfigId > 0 && Part <= 10)
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

        [JsonIgnore]
        /// <summary>
        /// 品质属性
        /// </summary>
        public IDictionary<int, long> QualityAttrList { get; set; }

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

            foreach (int attrId in QualityAttrList.Keys)
            {
                AttrList[attrId] = QualityAttrList[attrId];
            }
            return AttrList;
        }
    }
}
