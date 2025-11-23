using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Linq;
using Game.Data;

namespace Game
{
    public class Shengxiao : Item
    {

        public int Quality { get; set; }

        public List<KeyValuePair<int, long>> AttrEntryList { get; set; } = new List<KeyValuePair<int, long>>();

        public MagicData LevelData { get; set; } = new MagicData();

        public MagicData LayerData { get; set; } = new MagicData();

        [JsonIgnore]
        public ShengxiaoConfig ShengxiaoConfig { get; set; }

        public override int GetQuality()
        {
            return Quality;
        }

        public Shengxiao(int configId, int quality)
        {
            this.Type = ItemType.Shengxiao;
            this.ConfigId = configId;

            this.ShengxiaoConfig = ShengxiaoConfigCategory.Instance.Get(configId);

            this.Name = this.ShengxiaoConfig.Name;
            Quality = quality;
        }

        public void Init(List<KeyValuePair<int, long>> list)
        {
            AttrEntryList.AddRange(list);
        }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, long> GetTotalAttrList()
        {
            IDictionary<int, long> AttrList = new Dictionary<int, long>();

            //根据基础属性和词条属性，计算总属性
            IDictionary<int, long> BaseAttrList = this.GetBaseAttrList();

            foreach (int attrId in BaseAttrList.Keys)
            {
                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += BaseAttrList[attrId];
            }

            //计算随机属性
            long layer = this.LayerData.Data;
            for (int i = 0; i < AttrEntryList.Count; i++)
            {
                int attrId = AttrEntryList[i].Key;
                long attrTotalValue = AttrEntryList[i].Value + ShengxiaoConfig.LayerValueList[i] * layer;

                if (!AttrList.ContainsKey(attrId))
                {
                    AttrList[attrId] = 0;
                }

                AttrList[attrId] += attrTotalValue;
            }

            return AttrList;
        }

        private int[] QualityRate = { 1, 2, 3, 4, 5, 10, 20, 30, 40 };

        public IDictionary<int, long> GetBaseAttrList()
        {
            long level = this.LevelData.Data;

            IDictionary<int, long> BaseAttrList = new Dictionary<int, long>();

            for (int i = 0; i < ShengxiaoConfig.AttrIdList.Length; i++)
            {
                BaseAttrList.Add(ShengxiaoConfig.AttrIdList[i], ShengxiaoConfig.AttrValueList[i] * QualityRate[Quality - 1] + ShengxiaoConfig.AttchValueList[i] * level);
            }

            return BaseAttrList;
        }



        public void Up()
        {
            this.LevelData.Data++;
        }
        public void Grade()
        {
            this.LayerData.Data++;

        }
    }
}
