using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipSpeicalConfigCategory : ProtoObject, IMerge
    {
        public static EquipSpeicalConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipSpeicalConfig> dict = new Dictionary<int, EquipSpeicalConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipSpeicalConfig> list = new List<EquipSpeicalConfig>();
		
        public EquipSpeicalConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipSpeicalConfigCategory s = o as EquipSpeicalConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipSpeicalConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipSpeicalConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipSpeicalConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipSpeicalConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipSpeicalConfig> GetAll()
        {
            return this.dict;
        }

        public EquipSpeicalConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipSpeicalConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(2)]
		public int Layer { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(3)]
		public int ItemId { get; set; }
		/// <summary>基础属性列表</summary>
		[ProtoMember(4)]
		public int[] BaseArray { get; set; }
		/// <summary>基础属性值</summary>
		[ProtoMember(5)]
		public long[] AttributeBase { get; set; }
		/// <summary>品质</summary>
		[ProtoMember(6)]
		public int Quality { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(7)]
		public long Fee { get; set; }
		/// <summary>FeeItemId</summary>
		[ProtoMember(8)]
		public int FeeItemId { get; set; }

	}
}
