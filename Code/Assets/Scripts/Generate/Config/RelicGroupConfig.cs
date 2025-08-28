using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class RelicGroupConfigCategory : ProtoObject, IMerge
    {
        public static RelicGroupConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RelicGroupConfig> dict = new Dictionary<int, RelicGroupConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RelicGroupConfig> list = new List<RelicGroupConfig>();
		
        public RelicGroupConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            RelicGroupConfigCategory s = o as RelicGroupConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (RelicGroupConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RelicGroupConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RelicGroupConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RelicGroupConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RelicGroupConfig> GetAll()
        {
            return this.dict;
        }

        public RelicGroupConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RelicGroupConfig: ProtoObject, IConfig
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
		public double AttrValue { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(5)]
		public double RiseAttr { get; set; }
		/// <summary>RiseType</summary>
		[ProtoMember(6)]
		public double RiseType { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(7)]
		public string Des { get; set; }

	}
}
