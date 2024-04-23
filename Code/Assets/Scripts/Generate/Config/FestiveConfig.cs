using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FestiveConfigCategory : ProtoObject, IMerge
    {
        public static FestiveConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FestiveConfig> dict = new Dictionary<int, FestiveConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FestiveConfig> list = new List<FestiveConfig>();
		
        public FestiveConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FestiveConfigCategory s = o as FestiveConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FestiveConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FestiveConfig Get(int id)
        {
            this.dict.TryGetValue(id, out FestiveConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FestiveConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FestiveConfig> GetAll()
        {
            return this.dict;
        }

        public FestiveConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FestiveConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Cost</summary>
		[ProtoMember(2)]
		public int Cost { get; set; }
		/// <summary>Max</summary>
		[ProtoMember(3)]
		public int Max { get; set; }
		/// <summary>Title</summary>
		[ProtoMember(4)]
		public string Title { get; set; }
		/// <summary>TargetName</summary>
		[ProtoMember(5)]
		public string TargetName { get; set; }
		/// <summary>TargetType</summary>
		[ProtoMember(6)]
		public int TargetType { get; set; }
		/// <summary>TargetId</summary>
		[ProtoMember(7)]
		public int TargetId { get; set; }
		/// <summary>TargetCount</summary>
		[ProtoMember(8)]
		public int TargetCount { get; set; }

	}
}
