using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class GiftPackConfigCategory : ProtoObject, IMerge
    {
        public static GiftPackConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GiftPackConfig> dict = new Dictionary<int, GiftPackConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GiftPackConfig> list = new List<GiftPackConfig>();
		
        public GiftPackConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GiftPackConfigCategory s = o as GiftPackConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (GiftPackConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GiftPackConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GiftPackConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GiftPackConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GiftPackConfig> GetAll()
        {
            return this.dict;
        }

        public GiftPackConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GiftPackConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>名称</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>物品类型</summary>
		[ProtoMember(3)]
		public int[] ItemTypeList { get; set; }
		/// <summary>物品Id</summary>
		[ProtoMember(4)]
		public int[] ItemIdList { get; set; }
		/// <summary>物品数量</summary>
		[ProtoMember(5)]
		public int[] ItemQuanlityList { get; set; }

	}
}
