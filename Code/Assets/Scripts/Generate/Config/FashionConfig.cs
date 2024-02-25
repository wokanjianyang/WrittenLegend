using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FashionConfigCategory : ProtoObject, IMerge
    {
        public static FashionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FashionConfig> dict = new Dictionary<int, FashionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FashionConfig> list = new List<FashionConfig>();
		
        public FashionConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FashionConfigCategory s = o as FashionConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FashionConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FashionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out FashionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FashionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FashionConfig> GetAll()
        {
            return this.dict;
        }

        public FashionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FashionConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(2)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(4)]
		public int Part { get; set; }
		/// <summary>SuitId</summary>
		[ProtoMember(5)]
		public int SuitId { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(6)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(7)]
		public int[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(8)]
		public int[] AttrRiseList { get; set; }

	}
}
