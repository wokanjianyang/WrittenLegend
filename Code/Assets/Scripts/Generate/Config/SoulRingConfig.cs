using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SoulRingConfigCategory : ProtoObject, IMerge
    {
        public static SoulRingConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SoulRingConfig> dict = new Dictionary<int, SoulRingConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SoulRingConfig> list = new List<SoulRingConfig>();
		
        public SoulRingConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SoulRingConfigCategory s = o as SoulRingConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SoulRingConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SoulRingConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SoulRingConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SoulRingConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SoulRingConfig> GetAll()
        {
            return this.dict;
        }

        public SoulRingConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SoulRingConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }

	}
}
