using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AurasConfigCategory : ProtoObject, IMerge
    {
        public static AurasConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AurasConfig> dict = new Dictionary<int, AurasConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AurasConfig> list = new List<AurasConfig>();
		
        public AurasConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AurasConfigCategory s = o as AurasConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AurasConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AurasConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AurasConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AurasConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AurasConfig> GetAll()
        {
            return this.dict;
        }

        public AurasConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AurasConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }

	}
}
