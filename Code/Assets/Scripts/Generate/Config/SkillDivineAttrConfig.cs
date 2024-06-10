using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SkillDivineAttrConfigCategory : ProtoObject, IMerge
    {
        public static SkillDivineAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillDivineAttrConfig> dict = new Dictionary<int, SkillDivineAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillDivineAttrConfig> list = new List<SkillDivineAttrConfig>();
		
        public SkillDivineAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillDivineAttrConfigCategory s = o as SkillDivineAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillDivineAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillDivineAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillDivineAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillDivineAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillDivineAttrConfig> GetAll()
        {
            return this.dict;
        }

        public SkillDivineAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillDivineAttrConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>SkillId</summary>
		[ProtoMember(2)]
		public int SkillId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(4)]
		public string Desc { get; set; }
		/// <summary>DamageType</summary>
		[ProtoMember(5)]
		public int DamageType { get; set; }
		/// <summary>Param</summary>
		[ProtoMember(6)]
		public int Param { get; set; }

	}
}
