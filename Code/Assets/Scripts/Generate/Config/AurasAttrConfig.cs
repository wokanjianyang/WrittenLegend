using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AurasAttrConfigCategory : ProtoObject, IMerge
    {
        public static AurasAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AurasAttrConfig> dict = new Dictionary<int, AurasAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AurasAttrConfig> list = new List<AurasAttrConfig>();
		
        public AurasAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AurasAttrConfigCategory s = o as AurasAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AurasAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AurasAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AurasAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AurasAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AurasAttrConfig> GetAll()
        {
            return this.dict;
        }

        public AurasAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AurasAttrConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>AurasId</summary>
		[ProtoMember(2)]
		public int AurasId { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public int AttrValue { get; set; }
		/// <summary>Rise</summary>
		[ProtoMember(6)]
		public int Rise { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(7)]
		public string Name { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(8)]
		public string Memo { get; set; }

	}
}
