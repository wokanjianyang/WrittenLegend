using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class OrbEffectConfigCategory : ProtoObject, IMerge
    {
        public static OrbEffectConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, OrbEffectConfig> dict = new Dictionary<int, OrbEffectConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<OrbEffectConfig> list = new List<OrbEffectConfig>();
		
        public OrbEffectConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            OrbEffectConfigCategory s = o as OrbEffectConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (OrbEffectConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public OrbEffectConfig Get(int id)
        {
            this.dict.TryGetValue(id, out OrbEffectConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (OrbEffectConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, OrbEffectConfig> GetAll()
        {
            return this.dict;
        }

        public OrbEffectConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class OrbEffectConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>CD</summary>
		[ProtoMember(2)]
		public int CD { get; set; }
		/// <summary>技能名字</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(4)]
		public int Type { get; set; }
		/// <summary>计算类型</summary>
		[ProtoMember(5)]
		public int CalType { get; set; }
		/// <summary>来源属性</summary>
		[ProtoMember(6)]
		public int SourceAttr { get; set; }
		/// <summary>优先度</summary>
		[ProtoMember(7)]
		public int Priority { get; set; }
		/// <summary>施法目标</summary>
		[ProtoMember(8)]
		public int TargetType { get; set; }
		/// <summary>运行类型</summary>
		[ProtoMember(9)]
		public int RunType { get; set; }
		/// <summary>目标属性</summary>
		[ProtoMember(10)]
		public int TargetAttr { get; set; }
		/// <summary>精通技能增益</summary>
		[ProtoMember(11)]
		public long ExpertRise { get; set; }
		/// <summary>等级增益</summary>
		[ProtoMember(12)]
		public int LevelRise { get; set; }
		/// <summary>备注</summary>
		[ProtoMember(13)]
		public string Des { get; set; }

	}
}
