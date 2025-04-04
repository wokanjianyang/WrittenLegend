using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class RelicDropConfigCategory : ProtoObject, IMerge
    {
        public static RelicDropConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RelicDropConfig> dict = new Dictionary<int, RelicDropConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RelicDropConfig> list = new List<RelicDropConfig>();
		
        public RelicDropConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            RelicDropConfigCategory s = o as RelicDropConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (RelicDropConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RelicDropConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RelicDropConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RelicDropConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RelicDropConfig> GetAll()
        {
            return this.dict;
        }

        public RelicDropConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RelicDropConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>DropType</summary>
		[ProtoMember(3)]
		public int DropType { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(4)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(5)]
		public string Name { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(6)]
		public int Rate { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(7)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(8)]
		public int EndLevel { get; set; }
		/// <summary>Max</summary>
		[ProtoMember(9)]
		public int Max { get; set; }

	}
}
