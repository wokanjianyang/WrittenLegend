using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

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
        public int Layer { get; set; } = 1;
        public int RuneSeed { get; set; }
        public int SuitSeed { get; set; }
        public int RefreshCount { get; set; }
        public long RefreshDate { get; set; }

        public Dictionary<int, int> HoneList { get; set; } = new Dictionary<int, int>();

        public EquipData Data { get; set; } = new EquipData();

        public override int GetQuality()
        {
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
        /// 品质属性
        /// </summary>
        public IDictionary<int, long> QualityAttrList { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 套装属性
        /// </summary>
        public IDictionary<int, long> GroupAttrList { get; set; }

        [JsonIgnore]
        /// <summary>
        /// 套装属性名称
        /// </summary>
        public IDictionary<int, string> GroupNameList { get; set; }

        public Equip(int configId, int runeConfigId, int suitConfigId, int quality)
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

            QualityAttrList = new Dictionary<int, long>();
            if (Quality > 0 && Part <= 10)
            {
                EquipQualityConfig qualityConfig = EquipQualityConfigCategory.Instance.Get(quality);

                if (qualityConfig.AttrIdList != null)
                {
                    int qualityRate = Level / 200 + 1;
                    for (int i = 0; i < qualityConfig.AttrIdList.Length; i++)
                    {
                        QualityAttrList.Add(qualityConfig.AttrIdList[i], qualityConfig.AttrValueList[i] * qualityRate);
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

        public IDictionary<int, long> GetBaseAttrList()
        {
            IDictionary<int, long> BaseAttrList = new Dictionary<int, long>();
            for (int i = 0; i < EquipConfig.AttributeBase.Length; i++)
            {
                long AttributeBase = EquipConfig.AttributeBase[i];

                if (this.Part <= 10)
                {
                    if (Quality <= 4)
                    {
                        AttributeBase = AttributeBase * (Quality * 20 + 20) / 100;
                    }
                    else if (Quality == 5)
                    {
                        AttributeBase = AttributeBase * 2;
                    }
                    else if (Quality == 6)
                    {
                        AttributeBase = AttributeBase * GetLayerRate(Layer);
                    }
                }

                BaseAttrList.Add(EquipConfig.BaseArray[i], AttributeBase);
            }

            return BaseAttrList;
        }

        private int GetLayerRate(int layer)
        {
            int b = 1;
            for (int i = 1; i < layer; i++)
            {
                b = b * 2;
            }
            return b;
        }

        public void CheckReFreshCount()
        {
            long tk = DateTime.Today.Ticks;
            if (this.RefreshDate < tk)
            {
                this.RefreshDate = tk;
                this.RefreshCount = ConfigHelper.EquipRefreshCount;
            }

            if (this.RuneSeed <= 0)
            {
                this.RuneSeed = AppHelper.InitSeed();
            }
            if (this.SuitSeed <= 0)
            {
                this.SuitSeed = AppHelper.InitSeed();
            }

            if (this.Data == null || this.Data.RuneIdList.Count == 0)
            {
                this.Data = new EquipData();
                this.Data.Refresh(this.Part, this.Level, this.Quality, this.EquipConfig.Role);
            }
        }

        public void Refesh(bool save)
        {
            if (save)
            {
                this.AttrEntryList.Clear();
                this.AttrEntryList.AddRange(Data.GetAttrList());

                this.RuneConfigId = Data.GetRuneId();
                this.SuitConfigId = Data.GetSuitId();

                this.SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(RuneConfigId);
                this.SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(SuitConfigId);
            }

            Data.Refresh(this.Part, this.Level, this.Quality, this.EquipConfig.Role);
        }

        public void Init(int seed)
        {
            //根据品质,生成随机属性
            if (EquipConfig.RandomAttr == 0 && Part <= 10)
            {
                this.AttrEntryList.AddRange(AttrEntryConfigCategory.Instance.Build(this.Part, this.Level, this.Quality, this.EquipConfig.Role, seed));
            }

            if (this.Part <= 10 && this.Quality >= 6)
            {
                this.Data = new EquipData();
                this.Data.Refresh(this.Part, this.Level, this.Quality, this.EquipConfig.Role);
            }
        }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, long> GetTotalAttrList(long level)
        {
            long basePercent = 100;
            long qualityPercent = 100;

            if (level > 0)
            {
                EquipRefineConfig refineConfig = EquipRefineConfigCategory.Instance.GetByLevel(level);
                basePercent += refineConfig.GetBaseAttrPercent(level);
                qualityPercent += refineConfig.GetQualityAttrPercent(level);
            }

            //根据基础属性和词条属性，计算总属性
            IDictionary<int, long> BaseAttrList = this.GetBaseAttrList();

            IDictionary<int, long> AttrList = new Dictionary<int, long>();
            foreach (int attrId in BaseAttrList.Keys)
            {
                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += BaseAttrList[attrId] * basePercent / 100;
            }

            for (int i = 0; i < AttrEntryList.Count; i++)
            {
                int attrId = AttrEntryList[i].Key;
                long attrTotalValue = AttrEntryList[i].Value + GetHoneValue(i);
                attrTotalValue = attrTotalValue * qualityPercent / 100;

                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += attrTotalValue;
            }

            foreach (int attrId in QualityAttrList.Keys)
            {
                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += QualityAttrList[attrId];
            }

            return AttrList;
        }

        public void Grade()
        {
            this.Layer++;
        }

        public void Hone(int index)
        {
            if (!HoneList.ContainsKey(index))
            {
                HoneList[index] = 0;
            }

            HoneList[index]++;
        }

        public int GetHoneLevel(int index)
        {
            if (HoneList.ContainsKey(index))
            {
                return HoneList[index];
            }
            return 0;
        }

        public int GetHoneValue(int index)
        {
            int level = GetHoneLevel(index);
            int attrId = this.AttrEntryList[index].Key;
            int attrValue = (int)this.AttrEntryList[index].Value;

            if (level > 0)
            {
                EquipHoneConfig config = EquipHoneConfigCategory.Instance.GetByAttrId(attrId);
                return Math.Min(level * config.AttrValue, config.StartValue + Layer * config.AttrValue - attrValue);
            }

            return 0;
        }
    }
}
