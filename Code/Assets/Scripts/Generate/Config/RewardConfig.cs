using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class RewardConfigCategory : ProtoObject, IMerge
    {
        public static RewardConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RewardConfig> dict = new Dictionary<int, RewardConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RewardConfig> list = new List<RewardConfig>();
		
        public RewardConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            RewardConfigCategory s = o as RewardConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (RewardConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RewardConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RewardConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RewardConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RewardConfig> GetAll()
        {
            return this.dict;
        }

        public RewardConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RewardConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(2)]
		public int type { get; set; }
		/// <summary>物品ID</summary>
		[ProtoMember(3)]
		public int ItemId { get; set; }

	}
}
