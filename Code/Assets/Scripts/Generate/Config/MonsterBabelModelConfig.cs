using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterBabelModelConfigCategory : ProtoObject, IMerge
    {
        public static MonsterBabelModelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterBabelModelConfig> dict = new Dictionary<int, MonsterBabelModelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterBabelModelConfig> list = new List<MonsterBabelModelConfig>();
		
        public MonsterBabelModelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterBabelModelConfigCategory s = o as MonsterBabelModelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterBabelModelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterBabelModelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterBabelModelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterBabelModelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterBabelModelConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterBabelModelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterBabelModelConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(3)]
		public int EndLevel { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(4)]
		public int Type { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(5)]
		public int[] SkillIdList { get; set; }

	}
}
