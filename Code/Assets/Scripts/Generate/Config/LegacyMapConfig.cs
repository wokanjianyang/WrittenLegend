using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyMapConfigCategory : ProtoObject, IMerge
    {
        public static LegacyMapConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyMapConfig> dict = new Dictionary<int, LegacyMapConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyMapConfig> list = new List<LegacyMapConfig>();
		
        public LegacyMapConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyMapConfigCategory s = o as LegacyMapConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyMapConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyMapConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyMapConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyMapConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyMapConfig> GetAll()
        {
            return this.dict;
        }

        public LegacyMapConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyMapConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>PowerList</summary>
		[ProtoMember(3)]
		public int[] PowerList { get; set; }

	}
}
