using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterBabelMythConfigCategory : ProtoObject, IMerge
    {
        public static MonsterBabelMythConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterBabelMythConfig> dict = new Dictionary<int, MonsterBabelMythConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterBabelMythConfig> list = new List<MonsterBabelMythConfig>();
		
        public MonsterBabelMythConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterBabelMythConfigCategory s = o as MonsterBabelMythConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterBabelMythConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterBabelMythConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterBabelMythConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterBabelMythConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterBabelMythConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterBabelMythConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterBabelMythConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(3)]
		public int EndLevel { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(4)]
		public string Attr { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(5)]
		public double AttrRise { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(6)]
		public string Def { get; set; }
		/// <summary>DefRise</summary>
		[ProtoMember(7)]
		public double DefRise { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(8)]
		public string HP { get; set; }
		/// <summary>HpRise</summary>
		[ProtoMember(9)]
		public double HpRise { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(10)]
		public int Speed { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(11)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(12)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(13)]
		public int CritRateResist { get; set; }
		/// <summary>CritDamageResist</summary>
		[ProtoMember(14)]
		public int CritDamageResist { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(15)]
		public int Protect { get; set; }

	}
}
