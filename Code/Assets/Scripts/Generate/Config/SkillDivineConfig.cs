using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SkillDivineConfigCategory : ProtoObject, IMerge
    {
        public static SkillDivineConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillDivineConfig> dict = new Dictionary<int, SkillDivineConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillDivineConfig> list = new List<SkillDivineConfig>();
		
        public SkillDivineConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillDivineConfigCategory s = o as SkillDivineConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillDivineConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillDivineConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillDivineConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillDivineConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillDivineConfig> GetAll()
        {
            return this.dict;
        }

        public SkillDivineConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillDivineConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(3)]
		public int ItemId { get; set; }
		/// <summary>SkillAttrId</summary>
		[ProtoMember(4)]
		public int SkillAttrId { get; set; }
		/// <summary>SkillAttrValue</summary>
		[ProtoMember(5)]
		public int SkillAttrValue { get; set; }

	}
}
