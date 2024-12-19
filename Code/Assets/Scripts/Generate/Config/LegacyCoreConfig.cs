using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyCoreConfigCategory : ProtoObject, IMerge
    {
        public static LegacyCoreConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyCoreConfig> dict = new Dictionary<int, LegacyCoreConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyCoreConfig> list = new List<LegacyCoreConfig>();
		
        public LegacyCoreConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyCoreConfigCategory s = o as LegacyCoreConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyCoreConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyCoreConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyCoreConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyCoreConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyCoreConfig> GetAll()
        {
            return this.dict;
        }

        public LegacyCoreConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyCoreConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>LevelRequire</summary>
		[ProtoMember(2)]
		public int LevelRequire { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(3)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(4)]
		public double AttrValue { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(5)]
		public int Fee { get; set; }

	}
}
