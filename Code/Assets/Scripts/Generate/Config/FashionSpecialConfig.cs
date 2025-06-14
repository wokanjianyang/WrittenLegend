using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class FashionSpecialConfigCategory : ProtoObject, IMerge
    {
        public static FashionSpecialConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, FashionSpecialConfig> dict = new Dictionary<int, FashionSpecialConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<FashionSpecialConfig> list = new List<FashionSpecialConfig>();
		
        public FashionSpecialConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            FashionSpecialConfigCategory s = o as FashionSpecialConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (FashionSpecialConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public FashionSpecialConfig Get(int id)
        {
            this.dict.TryGetValue(id, out FashionSpecialConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (FashionSpecialConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, FashionSpecialConfig> GetAll()
        {
            return this.dict;
        }

        public FashionSpecialConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class FashionSpecialConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(3)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public int[] AttrValueList { get; set; }
		/// <summary>UpAttrId</summary>
		[ProtoMember(5)]
		public int UpAttrId { get; set; }
		/// <summary>UpAttrValue</summary>
		[ProtoMember(6)]
		public int UpAttrValue { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(7)]
		public int Fee { get; set; }

	}
}
