using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SevenDayConfigCategory : ProtoObject, IMerge
    {
        public static SevenDayConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SevenDayConfig> dict = new Dictionary<int, SevenDayConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SevenDayConfig> list = new List<SevenDayConfig>();
		
        public SevenDayConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SevenDayConfigCategory s = o as SevenDayConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SevenDayConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SevenDayConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SevenDayConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SevenDayConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SevenDayConfig> GetAll()
        {
            return this.dict;
        }

        public SevenDayConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SevenDayConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Day</summary>
		[ProtoMember(2)]
		public int Day { get; set; }
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
