using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class WorldDropConfigCategory : ProtoObject, IMerge
    {
        public static WorldDropConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, WorldDropConfig> dict = new Dictionary<int, WorldDropConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<WorldDropConfig> list = new List<WorldDropConfig>();
		
        public WorldDropConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            WorldDropConfigCategory s = o as WorldDropConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (WorldDropConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public WorldDropConfig Get(int id)
        {
            this.dict.TryGetValue(id, out WorldDropConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (WorldDropConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, WorldDropConfig> GetAll()
        {
            return this.dict;
        }

        public WorldDropConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class WorldDropConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(3)]
		public int ItemId { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(4)]
		public int Rate { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(5)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(6)]
		public int EndLevel { get; set; }
		/// <summary>RateLevel</summary>
		[ProtoMember(7)]
		public int RateLevel { get; set; }
		/// <summary>ExcludeStart</summary>
		[ProtoMember(8)]
		public int ExcludeStart { get; set; }
		/// <summary>ExcludeLevel</summary>
		[ProtoMember(9)]
		public int ExcludeLevel { get; set; }

	}
}
