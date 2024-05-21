using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyConfigCategory : ProtoObject, IMerge
    {
        public static LegacyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyConfig> dict = new Dictionary<int, LegacyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyConfig> list = new List<LegacyConfig>();
		
        public LegacyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyConfigCategory s = o as LegacyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyConfig> GetAll()
        {
            return this.dict;
        }

        public LegacyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(2)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(4)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(5)]
		public int[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(6)]
		public int[] AttrRiseList { get; set; }
		/// <summary>LayerIdList</summary>
		[ProtoMember(7)]
		public int[] LayerIdList { get; set; }
		/// <summary>LayerValueList</summary>
		[ProtoMember(8)]
		public int[] LayerValueList { get; set; }
		/// <summary>LayerRiseList</summary>
		[ProtoMember(9)]
		public int[] LayerRiseList { get; set; }
		/// <summary>Power1</summary>
		[ProtoMember(10)]
		public int Power1 { get; set; }
		/// <summary>Power2</summary>
		[ProtoMember(11)]
		public int Power2 { get; set; }
		/// <summary>Power3</summary>
		[ProtoMember(12)]
		public int Power3 { get; set; }

	}
}
