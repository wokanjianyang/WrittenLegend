using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AbyssDropConfigCategory : ProtoObject, IMerge
    {
        public static AbyssDropConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AbyssDropConfig> dict = new Dictionary<int, AbyssDropConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AbyssDropConfig> list = new List<AbyssDropConfig>();
		
        public AbyssDropConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AbyssDropConfigCategory s = o as AbyssDropConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AbyssDropConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AbyssDropConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AbyssDropConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AbyssDropConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AbyssDropConfig> GetAll()
        {
            return this.dict;
        }

        public AbyssDropConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AbyssDropConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(3)]
		public int MapId { get; set; }
		/// <summary>Stage</summary>
		[ProtoMember(4)]
		public int Stage { get; set; }
		/// <summary>DropId</summary>
		[ProtoMember(5)]
		public int DropId { get; set; }
		/// <summary>DropRate</summary>
		[ProtoMember(6)]
		public int DropRate { get; set; }

	}
}
