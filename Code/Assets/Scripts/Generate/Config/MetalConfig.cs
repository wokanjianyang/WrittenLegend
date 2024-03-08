using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MetalConfigCategory : ProtoObject, IMerge
    {
        public static MetalConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MetalConfig> dict = new Dictionary<int, MetalConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MetalConfig> list = new List<MetalConfig>();
		
        public MetalConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MetalConfigCategory s = o as MetalConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MetalConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MetalConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MetalConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MetalConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MetalConfig> GetAll()
        {
            return this.dict;
        }

        public MetalConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MetalConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(3)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(4)]
		public long AttrValue { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(5)]
		public int RiseAttr { get; set; }
		/// <summary>RiseLog</summary>
		[ProtoMember(6)]
		public int RiseLog { get; set; }
		/// <summary>RisePower</summary>
		[ProtoMember(7)]
		public int RisePower { get; set; }
		/// <summary>MaxLevel</summary>
		[ProtoMember(8)]
		public int MaxLevel { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(9)]
		public int Quality { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(10)]
		public string Des { get; set; }

	}
}
