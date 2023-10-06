using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class DropLimitConfigCategory : ProtoObject, IMerge
    {
        public static DropLimitConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DropLimitConfig> dict = new Dictionary<int, DropLimitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DropLimitConfig> list = new List<DropLimitConfig>();
		
        public DropLimitConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            DropLimitConfigCategory s = o as DropLimitConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (DropLimitConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DropLimitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DropLimitConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DropLimitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DropLimitConfig> GetAll()
        {
            return this.dict;
        }

        public DropLimitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DropLimitConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>掉落名称</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>掉落数量</summary>
		[ProtoMember(3)]
		public int Quantity { get; set; }
		/// <summary>道具类型</summary>
		[ProtoMember(4)]
		public int ItemType { get; set; }
		/// <summary>掉落Id</summary>
		[ProtoMember(5)]
		public int ItemId { get; set; }
		/// <summary>掉落概率</summary>
		[ProtoMember(6)]
		public int Rate { get; set; }
		/// <summary>开始日期</summary>
		[ProtoMember(7)]
		public string StartDate { get; set; }
		/// <summary>结束日期</summary>
		[ProtoMember(8)]
		public string EndDate { get; set; }

	}
}