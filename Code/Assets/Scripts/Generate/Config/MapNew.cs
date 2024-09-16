using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MapNewCategory : ProtoObject, IMerge
    {
        public static MapNewCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapNew> dict = new Dictionary<int, MapNew>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapNew> list = new List<MapNew>();
		
        public MapNewCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MapNewCategory s = o as MapNewCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MapNew config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MapNew Get(int id)
        {
            this.dict.TryGetValue(id, out MapNew item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MapNew)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapNew> GetAll()
        {
            return this.dict;
        }

        public MapNew GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapNew: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(2)]
		public string MapName { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(3)]
		public string MonsterName { get; set; }
		/// <summary>BosName</summary>
		[ProtoMember(4)]
		public string BosName { get; set; }

	}
}
