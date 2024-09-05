using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class DefendConfigCategory : ProtoObject, IMerge
    {
        public static DefendConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DefendConfig> dict = new Dictionary<int, DefendConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DefendConfig> list = new List<DefendConfig>();
		
        public DefendConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            DefendConfigCategory s = o as DefendConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (DefendConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DefendConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DefendConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DefendConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DefendConfig> GetAll()
        {
            return this.dict;
        }

        public DefendConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DefendConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(3)]
		public int Layer { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(4)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(5)]
		public int EndLevel { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(6)]
		public string Attr { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(7)]
		public string RiseAttr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(8)]
		public string Def { get; set; }
		/// <summary>RiseDef</summary>
		[ProtoMember(9)]
		public string RiseDef { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(10)]
		public string HP { get; set; }
		/// <summary>RiseHp</summary>
		[ProtoMember(11)]
		public string RiseHp { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(12)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(13)]
		public int DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(14)]
		public int CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(15)]
		public int CritDamage { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(16)]
		public int Accuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(17)]
		public int Miss { get; set; }
		/// <summary>MulDamageResist</summary>
		[ProtoMember(18)]
		public int MulDamageResist { get; set; }
		/// <summary>Exp</summary>
		[ProtoMember(19)]
		public double Exp { get; set; }
		/// <summary>Gold</summary>
		[ProtoMember(20)]
		public double Gold { get; set; }

	}
}
