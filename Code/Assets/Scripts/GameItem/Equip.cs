using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Game
{
    public class Equip : Item
    {
        public Equip(int configId,int runeConfigId)
        {
            this.Type = ItemType.Equip;
            this.ConfigId = configId;
            this.RuneConfigId = runeConfigId;

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

            if (RuneConfigId > 0) {
                SkillRuneConfig = SkillRuneConfigCategory.Instance.Get(RuneConfigId);

                if (SkillRuneConfig.SuitId > 0) {
                    SkillSuitConfig = SkillSuitConfigCategory.Instance.Get(SkillRuneConfig.SuitId);
                }
            }
        }

        /// <summary>
        /// ���������б�
        /// </summary>
        public List<KeyValuePair<int, long>> AttrEntryList { get; set; }

        public int RuneConfigId { get; set; }

        public int Quality { get; set; }

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
        /// ��������
        /// </summary>
        public IDictionary<int, long> BaseAttrList { get; set; }

        [JsonIgnore]
        /// <summary>
        /// �����б�
        /// </summary>
        public IDictionary<int, long> GetTotalAttrList
        {
            get
            {
                //���ݻ������Ժʹ������ԣ�����������
                IDictionary<int, long> AttrList = new Dictionary<int, long>();
                foreach (int attrId in BaseAttrList.Keys)
                {
                    AttrList[attrId] = BaseAttrList[attrId];
                }

                for (int i = 0; i < AttrEntryList.Count; i++)
                {
                    int attrId = AttrEntryList[i].Key;
                    long val = 0;
                    if (AttrList.TryGetValue(attrId, out val))
                    {
                    }
                    AttrList[attrId] = val + AttrEntryList[i].Value;
                }

                return AttrList;
            }
        }
    }
}