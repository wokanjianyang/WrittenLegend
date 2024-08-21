using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterBabelConfigCategory : ProtoObject, IMerge
    {
        public static MonsterBabelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterBabelConfig> dict = new Dictionary<int, MonsterBabelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterBabelConfig> list = new List<MonsterBabelConfig>();
		
        public MonsterBabelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterBabelConfigCategory s = o as MonsterBabelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterBabelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterBabelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterBabelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterBabelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterBabelConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterBabelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterBabelConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(4)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(5)]
		public int EndLevel { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(6)]
		public int[] SkillIdList { get; set; }
		/// <summary>AttrRate</summary>
		[ProtoMember(7)]
		public double AttrRate { get; set; }

	}
}
