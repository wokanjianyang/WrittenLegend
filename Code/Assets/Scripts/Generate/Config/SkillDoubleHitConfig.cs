using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SkillDoubleHitConfigCategory : ProtoObject, IMerge
    {
        public static SkillDoubleHitConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillDoubleHitConfig> dict = new Dictionary<int, SkillDoubleHitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillDoubleHitConfig> list = new List<SkillDoubleHitConfig>();
		
        public SkillDoubleHitConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillDoubleHitConfigCategory s = o as SkillDoubleHitConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillDoubleHitConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillDoubleHitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillDoubleHitConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillDoubleHitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillDoubleHitConfig> GetAll()
        {
            return this.dict;
        }

        public SkillDoubleHitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillDoubleHitConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>SkillId</summary>
		[ProtoMember(2)]
		public int SkillId { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(3)]
		public int Rate { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(5)]
		public string Des { get; set; }

	}
}
