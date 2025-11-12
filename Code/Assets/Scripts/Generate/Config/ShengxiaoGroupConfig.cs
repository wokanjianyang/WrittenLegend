using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ShengxiaoGroupConfigCategory : ProtoObject, IMerge
    {
        public static ShengxiaoGroupConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ShengxiaoGroupConfig> dict = new Dictionary<int, ShengxiaoGroupConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ShengxiaoGroupConfig> list = new List<ShengxiaoGroupConfig>();
		
        public ShengxiaoGroupConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ShengxiaoGroupConfigCategory s = o as ShengxiaoGroupConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ShengxiaoGroupConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ShengxiaoGroupConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ShengxiaoGroupConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ShengxiaoGroupConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ShengxiaoGroupConfig> GetAll()
        {
            return this.dict;
        }

        public ShengxiaoGroupConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ShengxiaoGroupConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(2)]
		public int Quality { get; set; }
		/// <summary>Count</summary>
		[ProtoMember(3)]
		public int Count { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public int AttrValue { get; set; }

	}
}
