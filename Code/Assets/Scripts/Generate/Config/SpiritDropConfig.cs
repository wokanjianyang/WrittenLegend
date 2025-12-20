using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SpiritDropConfigCategory : ProtoObject, IMerge
    {
        public static SpiritDropConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SpiritDropConfig> dict = new Dictionary<int, SpiritDropConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SpiritDropConfig> list = new List<SpiritDropConfig>();
		
        public SpiritDropConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SpiritDropConfigCategory s = o as SpiritDropConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SpiritDropConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SpiritDropConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SpiritDropConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SpiritDropConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SpiritDropConfig> GetAll()
        {
            return this.dict;
        }

        public SpiritDropConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SpiritDropConfig: ProtoObject, IConfig
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
