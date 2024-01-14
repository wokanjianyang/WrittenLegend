using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterModelConfigCategory : ProtoObject, IMerge
    {
        public static MonsterModelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterModelConfig> dict = new Dictionary<int, MonsterModelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterModelConfig> list = new List<MonsterModelConfig>();
		
        public MonsterModelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterModelConfigCategory s = o as MonsterModelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterModelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterModelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterModelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterModelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterModelConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterModelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterModelConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrRate</summary>
		[ProtoMember(3)]
		public double AttrRate { get; set; }
		/// <summary>HpRate</summary>
		[ProtoMember(4)]
		public double HpRate { get; set; }
		/// <summary>DefRate</summary>
		[ProtoMember(5)]
		public double DefRate { get; set; }
		/// <summary>DropRate</summary>
		[ProtoMember(6)]
		public double DropRate { get; set; }
		/// <summary>RewardRate</summary>
		[ProtoMember(7)]
		public double RewardRate { get; set; }
		/// <summary>CountRate</summary>
		[ProtoMember(8)]
		public double CountRate { get; set; }
		/// <summary>SkillList</summary>
		[ProtoMember(9)]
		public int[] SkillList { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(10)]
		public string Desc { get; set; }
		/// <summary>AttrList</summary>
		[ProtoMember(11)]
		public int[] AttrList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(12)]
		public long[] AttrValueList { get; set; }

	}
}
