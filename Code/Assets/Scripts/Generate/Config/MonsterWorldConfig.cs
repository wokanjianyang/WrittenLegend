using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterWorldConfigCategory : ProtoObject, IMerge
    {
        public static MonsterWorldConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterWorldConfig> dict = new Dictionary<int, MonsterWorldConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterWorldConfig> list = new List<MonsterWorldConfig>();
		
        public MonsterWorldConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterWorldConfigCategory s = o as MonsterWorldConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterWorldConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterWorldConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterWorldConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterWorldConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterWorldConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterWorldConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterWorldConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>Step</summary>
		[ProtoMember(3)]
		public int Step { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(4)]
		public string MonsterName { get; set; }
		/// <summary>LoseRate</summary>
		[ProtoMember(5)]
		public int LoseRate { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(6)]
		public int Speed { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(7)]
		public string Attr { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(8)]
		public double AttrRise { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(9)]
		public string Def { get; set; }
		/// <summary>DefRise</summary>
		[ProtoMember(10)]
		public double DefRise { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(11)]
		public string Hp { get; set; }
		/// <summary>RiseHp</summary>
		[ProtoMember(12)]
		public double RiseHp { get; set; }
		/// <summary>DamageMul</summary>
		[ProtoMember(13)]
		public string DamageMul { get; set; }
		/// <summary>MulRise</summary>
		[ProtoMember(14)]
		public double MulRise { get; set; }
		/// <summary>Strong</summary>
		[ProtoMember(15)]
		public string Strong { get; set; }
		/// <summary>StrongRise</summary>
		[ProtoMember(16)]
		public double StrongRise { get; set; }
		/// <summary>Parray</summary>
		[ProtoMember(17)]
		public string Parray { get; set; }
		/// <summary>ParrayRise</summary>
		[ProtoMember(18)]
		public double ParrayRise { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(19)]
		public int CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(20)]
		public int CritDamage { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(21)]
		public int CritRateResist { get; set; }
		/// <summary>CritDamageResist</summary>
		[ProtoMember(22)]
		public int CritDamageResist { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(23)]
		public int Accuracy { get; set; }
		/// <summary>AccuracyRise</summary>
		[ProtoMember(24)]
		public double AccuracyRise { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(25)]
		public int Miss { get; set; }
		/// <summary>MissRise</summary>
		[ProtoMember(26)]
		public double MissRise { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(27)]
		public int Protect { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(28)]
		public int[] SkillIdList { get; set; }
		/// <summary>SkillLevelList</summary>
		[ProtoMember(29)]
		public int[] SkillLevelList { get; set; }

	}
}
