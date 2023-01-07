using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ItemConfigCategory : ProtoObject, IMerge
    {
        public static ItemConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ItemConfig> dict = new Dictionary<int, ItemConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ItemConfig> list = new List<ItemConfig>();
		
        public ItemConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ItemConfigCategory s = o as ItemConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ItemConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ItemConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ItemConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ItemConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ItemConfig> GetAll()
        {
            return this.dict;
        }

        public ItemConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ItemConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>道具名字</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>道具类型</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>道具描述</summary>
		[ProtoMember(4)]
		public string Des { get; set; }
		/// <summary>售价</summary>
		[ProtoMember(5)]
		public int Price { get; set; }
		/// <summary>堆叠数量</summary>
		[ProtoMember(6)]
		public int MaxNum { get; set; }
		/// <summary>道具使用等级</summary>
		[ProtoMember(7)]
		public int LevelRequired { get; set; }
		/// <summary>品质</summary>
		[ProtoMember(8)]
		public int Quality { get; set; }
		/// <summary>使用效果值</summary>
		[ProtoMember(9)]
		public int UseParam { get; set; }

	}
}
