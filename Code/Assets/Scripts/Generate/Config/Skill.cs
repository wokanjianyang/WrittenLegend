using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SkillCategory : ProtoObject, IMerge
    {
        public static SkillCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Skill> dict = new Dictionary<int, Skill>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Skill> list = new List<Skill>();
		
        public SkillCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillCategory s = o as SkillCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Skill config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public Skill Get(int id)
        {
            this.dict.TryGetValue(id, out Skill item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Skill)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Skill> GetAll()
        {
            return this.dict;
        }

        public Skill GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Skill: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>技能ID</summary>
		[ProtoMember(2)]
		public int SkillId { get; set; }
		/// <summary>技能名字</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>技能描述</summary>
		[ProtoMember(4)]
		public string Des { get; set; }
		/// <summary>冷却时间</summary>
		[ProtoMember(5)]
		public int CD { get; set; }
		/// <summary>技能类型</summary>
		[ProtoMember(6)]
		public int SkillType { get; set; }
		/// <summary>伤害类型</summary>
		[ProtoMember(7)]
		public int Type { get; set; }
		/// <summary>技能等级</summary>
		[ProtoMember(8)]
		public int Level { get; set; }
		/// <summary>攻击距离</summary>
		[ProtoMember(9)]
		public int Dis { get; set; }
		/// <summary>攻击区域</summary>
		[ProtoMember(10)]
		public string Area { get; set; }
		/// <summary>#区域类型</summary>
		[ProtoMember(11)]
		public int AreaType { get; set; }
		/// <summary>最大敌人数量</summary>
		[ProtoMember(12)]
		public int EnemyNum { get; set; }
		/// <summary>伤害比例</summary>
		[ProtoMember(13)]
		public string Damage { get; set; }

	}
}
