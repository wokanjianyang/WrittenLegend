using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LevelConfigCategory : ProtoObject, IMerge
    {
        public static LevelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LevelConfig> dict = new Dictionary<int, LevelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LevelConfig> list = new List<LevelConfig>();
		
        public LevelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LevelConfigCategory s = o as LevelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LevelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LevelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LevelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LevelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LevelConfig> GetAll()
        {
            return this.dict;
        }

        public LevelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LevelConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public long StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(3)]
		public long EndLevel { get; set; }
		/// <summary>StartAttr</summary>
		[ProtoMember(4)]
		public long StartAttr { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(5)]
		public double RiseAttr { get; set; }
		/// <summary>StartDef</summary>
		[ProtoMember(6)]
		public long StartDef { get; set; }
		/// <summary>RiseDef</summary>
		[ProtoMember(7)]
		public double RiseDef { get; set; }
		/// <summary>StartHp</summary>
		[ProtoMember(8)]
		public long StartHp { get; set; }
		/// <summary>RiseHp</summary>
		[ProtoMember(9)]
		public long RiseHp { get; set; }
		/// <summary>StartExp</summary>
		[ProtoMember(10)]
		public long StartExp { get; set; }
		/// <summary>RiseExp</summary>
		[ProtoMember(11)]
		public long RiseExp { get; set; }

	}
}
