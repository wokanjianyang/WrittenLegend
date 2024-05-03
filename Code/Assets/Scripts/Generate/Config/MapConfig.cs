using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MapConfigCategory : ProtoObject, IMerge
    {
        public static MapConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapConfig> dict = new Dictionary<int, MapConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapConfig> list = new List<MapConfig>();
		
        public MapConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MapConfigCategory s = o as MapConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MapConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MapConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MapConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MapConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapConfig> GetAll()
        {
            return this.dict;
        }

        public MapConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Level</summary>
		[ProtoMember(3)]
		public int Level { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(4)]
		public int Layer { get; set; }
		/// <summary>进入等级要求</summary>
		[ProtoMember(5)]
		public int LevelRequired { get; set; }
		/// <summary>周围地图ID</summary>
		[ProtoMember(6)]
		public int[] MapAfter { get; set; }
		/// <summary>怪物最小等级</summary>
		[ProtoMember(7)]
		public int MonsterLevelMin { get; set; }
		/// <summary>怪物最大等级</summary>
		[ProtoMember(8)]
		public int MonsterLevelMax { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(9)]
		public string Memo { get; set; }
		/// <summary>DropLevel</summary>
		[ProtoMember(10)]
		public int DropLevel { get; set; }
		/// <summary>地图Id掉落</summary>
		[ProtoMember(11)]
		public int[] DropIdList { get; set; }
		/// <summary>DropRateList</summary>
		[ProtoMember(12)]
		public int[] DropRateList { get; set; }
		/// <summary>BossId</summary>
		[ProtoMember(13)]
		public int BoosId { get; set; }

	}
}
