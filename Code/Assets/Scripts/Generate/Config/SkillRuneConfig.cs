using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SkillRuneConfigCategory : ProtoObject, IMerge
    {
        public static SkillRuneConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillRuneConfig> dict = new Dictionary<int, SkillRuneConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillRuneConfig> list = new List<SkillRuneConfig>();
		
        public SkillRuneConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillRuneConfigCategory s = o as SkillRuneConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillRuneConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillRuneConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillRuneConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillRuneConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillRuneConfig> GetAll()
        {
            return this.dict;
        }

        public SkillRuneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillRuneConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>技能ID</summary>
		[ProtoMember(2)]
		public int SkillId { get; set; }
		/// <summary>词条名字</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>套装ID</summary>
		[ProtoMember(4)]
		public int SuitId { get; set; }
		/// <summary>技能描述</summary>
		[ProtoMember(5)]
		public string Des { get; set; }
		/// <summary>词条叠加数量</summary>
		[ProtoMember(6)]
		public int Max { get; set; }
		/// <summary>减少冷却时间</summary>
		[ProtoMember(7)]
		public int CD { get; set; }
		/// <summary>修改施法类型</summary>
		[ProtoMember(8)]
		public int CastType { get; set; }
		/// <summary>增加攻击距离</summary>
		[ProtoMember(9)]
		public int Dis { get; set; }
		/// <summary>修改攻击区域</summary>
		[ProtoMember(10)]
		public string Area { get; set; }
		/// <summary>增加最大敌人数量</summary>
		[ProtoMember(11)]
		public int EnemyMax { get; set; }
		/// <summary>增加伤害比例</summary>
		[ProtoMember(12)]
		public int Percent { get; set; }
		/// <summary>固定伤害</summary>
		[ProtoMember(13)]
		public int Damage { get; set; }
		/// <summary>附带效果</summary>
		[ProtoMember(14)]
		public int EffectId { get; set; }

	}
}