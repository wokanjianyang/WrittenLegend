using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class BossConfigCategory : ProtoObject, IMerge
    {
        public static BossConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BossConfig> dict = new Dictionary<int, BossConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BossConfig> list = new List<BossConfig>();
		
        public BossConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            BossConfigCategory s = o as BossConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (BossConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BossConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BossConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BossConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BossConfig> GetAll()
        {
            return this.dict;
        }

        public BossConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BossConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>所属地图</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>名称</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>攻击</summary>
		[ProtoMember(4)]
		public double PhyAttr { get; set; }
		/// <summary>防御</summary>
		[ProtoMember(5)]
		public double Def { get; set; }
		/// <summary>生命</summary>
		[ProtoMember(6)]
		public double HP { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(7)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(8)]
		public int DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(9)]
		public int CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(10)]
		public int CritDamage { get; set; }
		/// <summary>经验</summary>
		[ProtoMember(11)]
		public long Exp { get; set; }
		/// <summary>掉落金币</summary>
		[ProtoMember(12)]
		public long Gold { get; set; }
		/// <summary>地图Id掉落</summary>
		[ProtoMember(13)]
		public int[] DropIdList { get; set; }
		/// <summary>掉落概率列表</summary>
		[ProtoMember(14)]
		public int[] DropRateList { get; set; }
		/// <summary>模型</summary>
		[ProtoMember(15)]
		public int ModelType { get; set; }

	}
}
