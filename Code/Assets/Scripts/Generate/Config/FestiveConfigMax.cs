using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FestiveConfigMaxCategory : ProtoObject, IMerge
    {
        public static FestiveConfigMaxCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FestiveConfigMax> dict = new Dictionary<int, FestiveConfigMax>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FestiveConfigMax> list = new List<FestiveConfigMax>();
		
        public FestiveConfigMaxCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FestiveConfigMaxCategory s = o as FestiveConfigMaxCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FestiveConfigMax config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FestiveConfigMax Get(int id)
        {
            this.dict.TryGetValue(id, out FestiveConfigMax item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FestiveConfigMax)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FestiveConfigMax> GetAll()
        {
            return this.dict;
        }

        public FestiveConfigMax GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FestiveConfigMax: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Step</summary>
		[ProtoMember(2)]
		public int Step { get; set; }
		/// <summary>Cost</summary>
		[ProtoMember(3)]
		public int Cost { get; set; }
		/// <summary>Max</summary>
		[ProtoMember(4)]
		public int Max { get; set; }
		/// <summary>Title</summary>
		[ProtoMember(5)]
		public string Title { get; set; }
		/// <summary>TargetName</summary>
		[ProtoMember(6)]
		public string TargetName { get; set; }
		/// <summary>TargetType</summary>
		[ProtoMember(7)]
		public int TargetType { get; set; }
		/// <summary>TargetId</summary>
		[ProtoMember(8)]
		public int TargetId { get; set; }
		/// <summary>TargetCount</summary>
		[ProtoMember(9)]
		public int TargetCount { get; set; }

	}
}
