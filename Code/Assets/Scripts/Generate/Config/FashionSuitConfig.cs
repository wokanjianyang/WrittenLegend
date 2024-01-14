using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FashionSuitConfigCategory : ProtoObject, IMerge
    {
        public static FashionSuitConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FashionSuitConfig> dict = new Dictionary<int, FashionSuitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FashionSuitConfig> list = new List<FashionSuitConfig>();
		
        public FashionSuitConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FashionSuitConfigCategory s = o as FashionSuitConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FashionSuitConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FashionSuitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out FashionSuitConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FashionSuitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FashionSuitConfig> GetAll()
        {
            return this.dict;
        }

        public FashionSuitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FashionSuitConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(3)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(4)]
		public int AttrValue { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(5)]
		public int AttrRise { get; set; }
		/// <summary>MaxLevel</summary>
		[ProtoMember(6)]
		public int MaxLevel { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(7)]
		public int Part { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(8)]
		public int Type { get; set; }
		/// <summary>Level</summary>
		[ProtoMember(9)]
		public int Level { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(10)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(11)]
		public int[] AttrValueList { get; set; }
		/// <summary>品质</summary>
		[ProtoMember(12)]
		public int Quality { get; set; }
		/// <summary>词条</summary>
		[ProtoMember(13)]
		public int RuneId { get; set; }
		/// <summary>套装</summary>
		[ProtoMember(14)]
		public int SuitId { get; set; }

	}
}
