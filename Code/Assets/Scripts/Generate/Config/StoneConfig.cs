using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class StoneConfigCategory : ProtoObject, IMerge
    {
        public static StoneConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StoneConfig> dict = new Dictionary<int, StoneConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StoneConfig> list = new List<StoneConfig>();
		
        public StoneConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            StoneConfigCategory s = o as StoneConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (StoneConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public StoneConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StoneConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (StoneConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StoneConfig> GetAll()
        {
            return this.dict;
        }

        public StoneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StoneConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(3)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(5)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(6)]
		public int AttrValue { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(7)]
		public int AttrRise { get; set; }
		/// <summary>RiseType</summary>
		[ProtoMember(8)]
		public int RiseType { get; set; }

	}
}
