using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SkillEffectConfigCategory : ProtoObject, IMerge
    {
        public static SkillEffectConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillEffectConfig> dict = new Dictionary<int, SkillEffectConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillEffectConfig> list = new List<SkillEffectConfig>();
		
        public SkillEffectConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillEffectConfigCategory s = o as SkillEffectConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillEffectConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillEffectConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillEffectConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillEffectConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillEffectConfig> GetAll()
        {
            return this.dict;
        }

        public SkillEffectConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillEffectConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>EffectId</summary>
		[ProtoMember(2)]
		public int Name { get; set; }
		/// <summary>持续时间</summary>
		[ProtoMember(3)]
		public int Duration { get; set; }
		/// <summary>叠加层数</summary>
		[ProtoMember(4)]
		public int Max { get; set; }
		/// <summary>技能ID</summary>
		[ProtoMember(5)]
		public int SkillId { get; set; }
		/// <summary>模型类型</summary>
		[ProtoMember(6)]
		public string ModelName { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(7)]
		public int Type { get; set; }
		/// <summary>优先度</summary>
		[ProtoMember(8)]
		public int Priority { get; set; }
		/// <summary>技能描述</summary>
		[ProtoMember(9)]
		public string Des { get; set; }
		/// <summary>冷却时间</summary>
		[ProtoMember(10)]
		public int CD { get; set; }
		/// <summary>施法类型</summary>
		[ProtoMember(11)]
		public int CastType { get; set; }
		/// <summary>职业</summary>
		[ProtoMember(12)]
		public int Role { get; set; }
		/// <summary>技能等级</summary>
		[ProtoMember(13)]
		public int MaxLevel { get; set; }
		/// <summary>攻击距离</summary>
		[ProtoMember(14)]
		public int Dis { get; set; }
		/// <summary>中心目标</summary>
		[ProtoMember(15)]
		public string Center { get; set; }
		/// <summary>攻击区域</summary>
		[ProtoMember(16)]
		public string Area { get; set; }
		/// <summary>最大敌人数量</summary>
		[ProtoMember(17)]
		public int EnemyMax { get; set; }
		/// <summary>行</summary>
		[ProtoMember(18)]
		public int Row { get; set; }
		/// <summary>列</summary>
		[ProtoMember(19)]
		public int Column { get; set; }
		/// <summary>伤害比例</summary>
		[ProtoMember(20)]
		public int Percent { get; set; }
		/// <summary>固定伤害</summary>
		[ProtoMember(21)]
		public int Damage { get; set; }
		/// <summary>无视防御</summary>
		[ProtoMember(22)]
		public int IgnoreDef { get; set; }
		/// <summary>暴击率</summary>
		[ProtoMember(23)]
		public int CritRate { get; set; }
		/// <summary>暴击倍率</summary>
		[ProtoMember(24)]
		public int CritDamage { get; set; }
		/// <summary>伤害加成</summary>
		[ProtoMember(25)]
		public int DamageIncrea { get; set; }
		/// <summary>附带效果</summary>
		[ProtoMember(26)]
		public string[] EffectList { get; set; }
		/// <summary>升级经验</summary>
		[ProtoMember(27)]
		public int Exp { get; set; }

	}
}
