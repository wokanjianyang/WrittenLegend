using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EffectConfigCategory : ProtoObject, IMerge
    {
        public static EffectConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EffectConfig> dict = new Dictionary<int, EffectConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EffectConfig> list = new List<EffectConfig>();
		
        public EffectConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EffectConfigCategory s = o as EffectConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EffectConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EffectConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EffectConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EffectConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EffectConfig> GetAll()
        {
            return this.dict;
        }

        public EffectConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EffectConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>技能名字</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>计算类型</summary>
		[ProtoMember(4)]
		public int CalType { get; set; }
		/// <summary>来源属性</summary>
		[ProtoMember(5)]
		public int SourceAttr { get; set; }
		/// <summary>优先度</summary>
		[ProtoMember(6)]
		public int Priority { get; set; }
		/// <summary>施法目标</summary>
		[ProtoMember(7)]
		public int TargetType { get; set; }
		/// <summary>运行类型</summary>
		[ProtoMember(8)]
		public int RunType { get; set; }
		/// <summary>目标属性</summary>
		[ProtoMember(9)]
		public int TargetAttr { get; set; }
		/// <summary>精通技能增益</summary>
		[ProtoMember(10)]
		public long ExpertRise { get; set; }
		/// <summary>等级增益</summary>
		[ProtoMember(11)]
		public int LevelRise { get; set; }
		/// <summary>备注</summary>
		[ProtoMember(12)]
		public string Des { get; set; }

	}
}
