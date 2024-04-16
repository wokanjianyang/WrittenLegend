using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class InfiniteDropConfigCategory : ProtoObject, IMerge
    {
        public static InfiniteDropConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, InfiniteDropConfig> dict = new Dictionary<int, InfiniteDropConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<InfiniteDropConfig> list = new List<InfiniteDropConfig>();
		
        public InfiniteDropConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            InfiniteDropConfigCategory s = o as InfiniteDropConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (InfiniteDropConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public InfiniteDropConfig Get(int id)
        {
            this.dict.TryGetValue(id, out InfiniteDropConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (InfiniteDropConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, InfiniteDropConfig> GetAll()
        {
            return this.dict;
        }

        public InfiniteDropConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class InfiniteDropConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>DropId</summary>
		[ProtoMember(2)]
		public int DropId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
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
		/// <summary>Max</summary>
		[ProtoMember(8)]
		public int Max { get; set; }

	}
}
