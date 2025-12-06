using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FestiveAttrConfigCategory : ProtoObject, IMerge
    {
        public static FestiveAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FestiveAttrConfig> dict = new Dictionary<int, FestiveAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FestiveAttrConfig> list = new List<FestiveAttrConfig>();
		
        public FestiveAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FestiveAttrConfigCategory s = o as FestiveAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FestiveAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FestiveAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out FestiveAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FestiveAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FestiveAttrConfig> GetAll()
        {
            return this.dict;
        }

        public FestiveAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FestiveAttrConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public int StartLevel { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public double AttrValue { get; set; }

	}
}
