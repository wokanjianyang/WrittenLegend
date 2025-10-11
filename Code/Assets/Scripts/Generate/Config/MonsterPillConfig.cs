using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterPillConfigCategory : ProtoObject, IMerge
    {
        public static MonsterPillConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterPillConfig> dict = new Dictionary<int, MonsterPillConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterPillConfig> list = new List<MonsterPillConfig>();
		
        public MonsterPillConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterPillConfigCategory s = o as MonsterPillConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterPillConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterPillConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterPillConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterPillConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterPillConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterPillConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterPillConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(3)]
		public int Layer { get; set; }
		/// <summary>RequireCycle</summary>
		[ProtoMember(4)]
		public int RequireCycle { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(5)]
		public string MapName { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(6)]
		public string MonsterName { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(7)]
		public string Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(8)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(9)]
		public string HP { get; set; }
		/// <summary>Strong</summary>
		[ProtoMember(10)]
		public string Strong { get; set; }
		/// <summary>Parray</summary>
		[ProtoMember(11)]
		public string Parray { get; set; }
		/// <summary>DamageMul</summary>
		[ProtoMember(12)]
		public string DamageMul { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(13)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(14)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(15)]
		public int CritRateResist { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(16)]
		public int CritDamage { get; set; }
		/// <summary>ResotrePercent</summary>
		[ProtoMember(17)]
		public int ResotrePercent { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(18)]
		public int Miss { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(19)]
		public int Accuracy { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(20)]
		public int Protect { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(21)]
		public double Speed { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(22)]
		public int[] SkillIdList { get; set; }

	}
}
