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
		public double Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(6)]
		public double Def { get; set; }
		/// <summary>Hp</summary>
		[ProtoMember(7)]
		public double Hp { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(8)]
		public long DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(9)]
		public long DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(10)]
		public long CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(11)]
		public int CritDamage { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(12)]
		public double AttrRise { get; set; }
		/// <summary>AttrAdvanceRise</summary>
		[ProtoMember(13)]
		public double AttrAdvanceRise { get; set; }
		/// <summary>ResistType</summary>
		[ProtoMember(14)]
		public int ResistType { get; set; }
		/// <summary>RewardId</summary>
		[ProtoMember(15)]
		public int RewardId { get; set; }
		/// <summary>RewardBase</summary>
		[ProtoMember(16)]
		public int RewardBase { get; set; }
		/// <summary>RewardRise</summary>
		[ProtoMember(17)]
		public int RewardRise { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(18)]
		public int[] SkillIdList { get; set; }
		/// <summary>PhanSkillIdList</summary>
		[ProtoMember(19)]
		public int[] PhanSkillIdList { get; set; }

	}
}
