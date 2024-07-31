using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PillConfigCategory : ProtoObject, IMerge
    {
        public static PillConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PillConfig> dict = new Dictionary<int, PillConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PillConfig> list = new List<PillConfig>();
		
        public PillConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PillConfigCategory s = o as PillConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PillConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PillConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PillConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PillConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PillConfig> GetAll()
        {
            return this.dict;
        }

        public PillConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PillConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Position</summary>
		[ProtoMember(3)]
		public int Position { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public int AttrValue { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(6)]
		public int AttrRise { get; set; }
		/// <summary>FeeRise</summary>
		[ProtoMember(7)]
		public int FeeRise { get; set; }

	}
}
