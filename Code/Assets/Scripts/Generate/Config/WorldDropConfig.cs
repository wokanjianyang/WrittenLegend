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
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>ItemType</summary>
		[ProtoMember(5)]
		public int ItemType { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(6)]
		public int ItemId { get; set; }
		/// <summary>ItemCount</summary>
		[ProtoMember(7)]
		public int ItemCount { get; set; }
		/// <summary>ItemType1</summary>
		[ProtoMember(8)]
		public int ItemType1 { get; set; }
		/// <summary>ItemId1</summary>
		[ProtoMember(9)]
		public int ItemId1 { get; set; }
		/// <summary>ItemCount1</summary>
		[ProtoMember(10)]
		public int ItemCount1 { get; set; }
		/// <summary>ItemType2</summary>
		[ProtoMember(11)]
		public int ItemType2 { get; set; }
		/// <summary>ItemId2</summary>
		[ProtoMember(12)]
		public int ItemId2 { get; set; }
		/// <summary>ItemCount2</summary>
		[ProtoMember(13)]
		public int ItemCount2 { get; set; }

	}
}
