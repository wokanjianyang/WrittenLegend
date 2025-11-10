using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ShengxiaoCopyConfigCategory : ProtoObject, IMerge
    {
        public static ShengxiaoCopyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ShengxiaoCopyConfig> dict = new Dictionary<int, ShengxiaoCopyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ShengxiaoCopyConfig> list = new List<ShengxiaoCopyConfig>();
		
        public ShengxiaoCopyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ShengxiaoCopyConfigCategory s = o as ShengxiaoCopyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ShengxiaoCopyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ShengxiaoCopyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ShengxiaoCopyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ShengxiaoCopyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ShengxiaoCopyConfig> GetAll()
        {
            return this.dict;
        }

        public ShengxiaoCopyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ShengxiaoCopyConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(2)]
		public string MapName { get; set; }
		/// <summary>ItemType</summary>
		[ProtoMember(3)]
		public int[] ItemType { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(4)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemQuantity</summary>
		[ProtoMember(5)]
		public int[] ItemQuantity { get; set; }

	}
}
