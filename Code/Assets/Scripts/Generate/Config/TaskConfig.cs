using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class TaskConfigCategory : ProtoObject, IMerge
    {
        public static TaskConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, TaskConfig> dict = new Dictionary<int, TaskConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<TaskConfig> list = new List<TaskConfig>();
		
        public TaskConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            TaskConfigCategory s = o as TaskConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (TaskConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public TaskConfig Get(int id)
        {
            this.dict.TryGetValue(id, out TaskConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (TaskConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, TaskConfig> GetAll()
        {
            return this.dict;
        }

        public TaskConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class TaskConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Condition</summary>
		[ProtoMember(3)]
		public long Condition { get; set; }
		/// <summary>RewardGold</summary>
		[ProtoMember(4)]
		public long RewardGold { get; set; }
		/// <summary>RewardExp</summary>
		[ProtoMember(5)]
		public long RewardExp { get; set; }
		/// <summary>RewardIdList</summary>
		[ProtoMember(6)]
		public int[] RewardIdList { get; set; }
		/// <summary>RewardTypeList</summary>
		[ProtoMember(7)]
		public int[] RewardTypeList { get; set; }
		/// <summary>QuanlityList</summary>
		[ProtoMember(8)]
		public int[] QuanlityList { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(9)]
		public string Memo { get; set; }
		/// <summary>Sort</summary>
		[ProtoMember(10)]
		public int Sort { get; set; }

	}
}
