using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class CompositeConfigCategory : ProtoObject, IMerge
    {
        public static CompositeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CompositeConfig> dict = new Dictionary<int, CompositeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CompositeConfig> list = new List<CompositeConfig>();
		
        public CompositeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            CompositeConfigCategory s = o as CompositeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (CompositeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public CompositeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CompositeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CompositeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CompositeConfig> GetAll()
        {
            return this.dict;
        }

        public CompositeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CompositeConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public string Type { get; set; }
		/// <summary>FromName</summary>
		[ProtoMember(3)]
		public string FromName { get; set; }
		/// <summary>FromId</summary>
		[ProtoMember(4)]
		public int FromId { get; set; }
		/// <summary>FromItemType</summary>
		[ProtoMember(5)]
		public int FromItemType { get; set; }
		/// <summary>TargetName</summary>
		[ProtoMember(6)]
		public string TargetName { get; set; }
		/// <summary>TargetId</summary>
		[ProtoMember(7)]
		public int TargetId { get; set; }
		/// <summary>TargetType</summary>
		[ProtoMember(8)]
		public int TargetType { get; set; }
		/// <summary>Quantity</summary>
		[ProtoMember(9)]
		public long Quantity { get; set; }
		/// <summary>Commission</summary>
		[ProtoMember(10)]
		public long Commission { get; set; }

	}
}
