using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AchievementConfigCategory : ProtoObject, IMerge
    {
        public static AchievementConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AchievementConfig> dict = new Dictionary<int, AchievementConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AchievementConfig> list = new List<AchievementConfig>();
		
        public AchievementConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AchievementConfigCategory s = o as AchievementConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AchievementConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AchievementConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AchievementConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AchievementConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AchievementConfig> GetAll()
        {
            return this.dict;
        }

        public AchievementConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AchievementConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>Condition</summary>
		[ProtoMember(4)]
		public long Condition { get; set; }
		/// <summary>RewardType</summary>
		[ProtoMember(5)]
		public int RewardType { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(6)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(7)]
		public int AttrValue { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(8)]
		public string Memo { get; set; }

	}
}
