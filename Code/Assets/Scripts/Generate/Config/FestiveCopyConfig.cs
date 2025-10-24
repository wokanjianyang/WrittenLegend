using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FestiveCopyConfigCategory : ProtoObject, IMerge
    {
        public static FestiveCopyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FestiveCopyConfig> dict = new Dictionary<int, FestiveCopyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FestiveCopyConfig> list = new List<FestiveCopyConfig>();
		
        public FestiveCopyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FestiveCopyConfigCategory s = o as FestiveCopyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FestiveCopyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FestiveCopyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out FestiveCopyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FestiveCopyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FestiveCopyConfig> GetAll()
        {
            return this.dict;
        }

        public FestiveCopyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FestiveCopyConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(2)]
		public string MapName { get; set; }
		/// <summary>FirstItemType</summary>
		[ProtoMember(3)]
		public int[] FirstItemType { get; set; }
		/// <summary>FirstItemIdList</summary>
		[ProtoMember(4)]
		public int[] FirstItemIdList { get; set; }
		/// <summary>FirstItemQuantity</summary>
		[ProtoMember(5)]
		public int[] FirstItemQuantity { get; set; }
		/// <summary>ItemType</summary>
		[ProtoMember(6)]
		public int[] ItemType { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(7)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemQuantity</summary>
		[ProtoMember(8)]
		public int[] ItemQuantity { get; set; }

	}
}
