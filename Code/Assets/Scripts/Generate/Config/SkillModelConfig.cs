using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SkillModelConfigCategory : ProtoObject, IMerge
    {
        public static SkillModelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillModelConfig> dict = new Dictionary<int, SkillModelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillModelConfig> list = new List<SkillModelConfig>();
		
        public SkillModelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillModelConfigCategory s = o as SkillModelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillModelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillModelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillModelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillModelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillModelConfig> GetAll()
        {
            return this.dict;
        }

        public SkillModelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillModelConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>模型类型</summary>
		[ProtoMember(2)]
		public string ModelName { get; set; }
		/// <summary>施法时间</summary>
		[ProtoMember(3)]
		public double ModelTime { get; set; }
		/// <summary>是否缩放</summary>
		[ProtoMember(4)]
		public int ScaleType { get; set; }

	}
}
