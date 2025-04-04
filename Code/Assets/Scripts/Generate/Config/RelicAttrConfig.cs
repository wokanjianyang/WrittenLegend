using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class RelicAttrConfigCategory : ProtoObject, IMerge
    {
        public static RelicAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RelicAttrConfig> dict = new Dictionary<int, RelicAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RelicAttrConfig> list = new List<RelicAttrConfig>();
		
        public RelicAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            RelicAttrConfigCategory s = o as RelicAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (RelicAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RelicAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RelicAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RelicAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RelicAttrConfig> GetAll()
        {
            return this.dict;
        }

        public RelicAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RelicAttrConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(2)]
		public int ItemId { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(3)]
		public int Layer { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(5)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(6)]
		public double AttrValue { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(7)]
		public double RiseAttr { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(8)]
		public string Des { get; set; }

	}
}
