using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PhantomAttrConfigCategory : ProtoObject, IMerge
    {
        public static PhantomAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PhantomAttrConfig> dict = new Dictionary<int, PhantomAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PhantomAttrConfig> list = new List<PhantomAttrConfig>();
		
        public PhantomAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PhantomAttrConfigCategory s = o as PhantomAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PhantomAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PhantomAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PhantomAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PhantomAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PhantomAttrConfig> GetAll()
        {
            return this.dict;
        }

        public PhantomAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PhantomAttrConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>PhId</summary>
		[ProtoMember(2)]
		public int PhId { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(5)]
		public string Attr { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(6)]
		public double AttrRise { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(7)]
		public string Def { get; set; }
		/// <summary>DefRise</summary>
		[ProtoMember(8)]
		public double DefRise { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(9)]
		public string Hp { get; set; }
		/// <summary>HpRise</summary>
		[ProtoMember(10)]
		public double HpRise { get; set; }
		/// <summary>DamageMul</summary>
		[ProtoMember(11)]
		public string DamageMul { get; set; }
		/// <summary>MulRise</summary>
		[ProtoMember(12)]
		public double MulRise { get; set; }
		/// <summary>Strong</summary>
		[ProtoMember(13)]
		public string Strong { get; set; }
		/// <summary>StrongRise</summary>
		[ProtoMember(14)]
		public double StrongRise { get; set; }
		/// <summary>Parray</summary>
		[ProtoMember(15)]
		public string Parray { get; set; }
		/// <summary>ParrayRise</summary>
		[ProtoMember(16)]
		public double ParrayRise { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(17)]
		public long DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(18)]
		public long DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(19)]
		public long CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(20)]
		public int CritDamage { get; set; }
		/// <summary>AttrAdvanceRise</summary>
		[ProtoMember(21)]
		public double AttrAdvanceRise { get; set; }
		/// <summary>ResistType</summary>
		[ProtoMember(22)]
		public int ResistType { get; set; }
		/// <summary>RequireId</summary>
		[ProtoMember(23)]
		public int RequireId { get; set; }
		/// <summary>RequireValue</summary>
		[ProtoMember(24)]
		public int RequireValue { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(25)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(26)]
		public double[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(27)]
		public double[] AttrRiseList { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(28)]
		public int Speed { get; set; }
		/// <summary>SpeedRise</summary>
		[ProtoMember(29)]
		public double SpeedRise { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(30)]
		public int Accuracy { get; set; }
		/// <summary>AccuracyRise</summary>
		[ProtoMember(31)]
		public double AccuracyRise { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(32)]
		public int Miss { get; set; }
		/// <summary>MissRise</summary>
		[ProtoMember(33)]
		public double MissRise { get; set; }
		/// <summary>RewardId</summary>
		[ProtoMember(34)]
		public int RewardId { get; set; }
		/// <summary>RewardBase</summary>
		[ProtoMember(35)]
		public int RewardBase { get; set; }
		/// <summary>RewardRise</summary>
		[ProtoMember(36)]
		public int RewardRise { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(37)]
		public int[] SkillIdList { get; set; }
		/// <summary>PhanSkillIdList</summary>
		[ProtoMember(38)]
		public int[] PhanSkillIdList { get; set; }

	}
}
