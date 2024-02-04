using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class GiftPackExclusiveConfigCategory : ProtoObject, IMerge
    {
        public static GiftPackExclusiveConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GiftPackExclusiveConfig> dict = new Dictionary<int, GiftPackExclusiveConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GiftPackExclusiveConfig> list = new List<GiftPackExclusiveConfig>();
		
        public GiftPackExclusiveConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GiftPackExclusiveConfigCategory s = o as GiftPackExclusiveConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (GiftPackExclusiveConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GiftPackExclusiveConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GiftPackExclusiveConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GiftPackExclusiveConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GiftPackExclusiveConfig> GetAll()
        {
            return this.dict;
        }

        public GiftPackExclusiveConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GiftPackExclusiveConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>ExclusiveId</summary>
		[ProtoMember(3)]
		public int ExclusiveId { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(4)]
		public int Quality { get; set; }
		/// <summary>RuneId</summary>
		[ProtoMember(5)]
		public int RuneId { get; set; }
		/// <summary>SuitId</summary>
		[ProtoMember(6)]
		public int SuitId { get; set; }
		/// <summary>DoubeId</summary>
		[ProtoMember(7)]
		public int DoubeId { get; set; }

	}
}
