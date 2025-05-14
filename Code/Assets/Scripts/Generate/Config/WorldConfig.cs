using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class WorldConfigCategory : ProtoObject, IMerge
    {
        public static WorldConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, WorldConfig> dict = new Dictionary<int, WorldConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<WorldConfig> list = new List<WorldConfig>();
		
        public WorldConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            WorldConfigCategory s = o as WorldConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (WorldConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public WorldConfig Get(int id)
        {
            this.dict.TryGetValue(id, out WorldConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (WorldConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, WorldConfig> GetAll()
        {
            return this.dict;
        }

        public WorldConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class WorldConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(2)]
		public string MapName { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(3)]
		public int Cycle { get; set; }

	}
}
