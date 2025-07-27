using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FestiveConfigBackCategory : ProtoObject, IMerge
    {
        public static FestiveConfigBackCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FestiveConfigBack> dict = new Dictionary<int, FestiveConfigBack>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FestiveConfigBack> list = new List<FestiveConfigBack>();
		
        public FestiveConfigBackCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FestiveConfigBackCategory s = o as FestiveConfigBackCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FestiveConfigBack config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FestiveConfigBack Get(int id)
        {
            this.dict.TryGetValue(id, out FestiveConfigBack item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FestiveConfigBack)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FestiveConfigBack> GetAll()
        {
            return this.dict;
        }

        public FestiveConfigBack GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FestiveConfigBack: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Step</summary>
		[ProtoMember(2)]
		public int Step { get; set; }
		/// <summary>RequireCycle</summary>
		[ProtoMember(3)]
		public int RequireCycle { get; set; }
		/// <summary>Cost</summary>
		[ProtoMember(4)]
		public int Cost { get; set; }
		/// <summary>Max</summary>
		[ProtoMember(5)]
		public int Max { get; set; }
		/// <summary>Title</summary>
		[ProtoMember(6)]
		public string Title { get; set; }
		/// <summary>TargetName</summary>
		[ProtoMember(7)]
		public string TargetName { get; set; }
		/// <summary>TargetType</summary>
		[ProtoMember(8)]
		public int TargetType { get; set; }
		/// <summary>TargetId</summary>
		[ProtoMember(9)]
		public int TargetId { get; set; }
		/// <summary>TargetCount</summary>
		[ProtoMember(10)]
		public int TargetCount { get; set; }

	}
}
