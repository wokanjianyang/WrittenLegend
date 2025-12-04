using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterFestiveConfigCategory : ProtoObject, IMerge
    {
        public static MonsterFestiveConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterFestiveConfig> dict = new Dictionary<int, MonsterFestiveConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterFestiveConfig> list = new List<MonsterFestiveConfig>();
		
        public MonsterFestiveConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterFestiveConfigCategory s = o as MonsterFestiveConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterFestiveConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterFestiveConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterFestiveConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterFestiveConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterFestiveConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterFestiveConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterFestiveConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(2)]
		public string MonsterName { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(3)]
		public string Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(4)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(5)]
		public string HP { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(6)]
		public int Speed { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(7)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(8)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(9)]
		public int CritRateResist { get; set; }
		/// <summary>CritDamageResist</summary>
		[ProtoMember(10)]
		public int CritDamageResist { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(11)]
		public int Protect { get; set; }

	}
}
