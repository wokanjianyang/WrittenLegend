using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class CardSpecialConfigCategory : ProtoObject, IMerge
    {
        public static CardSpecialConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CardSpecialConfig> dict = new Dictionary<int, CardSpecialConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CardSpecialConfig> list = new List<CardSpecialConfig>();
		
        public CardSpecialConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            CardSpecialConfigCategory s = o as CardSpecialConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (CardSpecialConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public CardSpecialConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CardSpecialConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CardSpecialConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CardSpecialConfig> GetAll()
        {
            return this.dict;
        }

        public CardSpecialConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CardSpecialConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(3)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(5)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(6)]
		public int[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(7)]
		public int[] AttrRiseList { get; set; }

	}
}
