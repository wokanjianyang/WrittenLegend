using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class RingConfigCategory : ProtoObject, IMerge
    {
        public static RingConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RingConfig> dict = new Dictionary<int, RingConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RingConfig> list = new List<RingConfig>();
		
        public RingConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            RingConfigCategory s = o as RingConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (RingConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RingConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RingConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RingConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RingConfig> GetAll()
        {
            return this.dict;
        }

        public RingConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RingConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(2)]
		public int Role { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(3)]
		public int ItemId { get; set; }
		/// <summary>SkillId</summary>
		[ProtoMember(4)]
		public int SkillId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(5)]
		public string Name { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(6)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(7)]
		public int[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(8)]
		public int[] AttrRiseList { get; set; }

	}
}
