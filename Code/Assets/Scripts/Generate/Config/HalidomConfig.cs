using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class HalidomConfigCategory : ProtoObject, IMerge
    {
        public static HalidomConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, HalidomConfig> dict = new Dictionary<int, HalidomConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<HalidomConfig> list = new List<HalidomConfig>();
		
        public HalidomConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            HalidomConfigCategory s = o as HalidomConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (HalidomConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public HalidomConfig Get(int id)
        {
            this.dict.TryGetValue(id, out HalidomConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (HalidomConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, HalidomConfig> GetAll()
        {
            return this.dict;
        }

        public HalidomConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class HalidomConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(2)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public long AttrValue { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(6)]
		public long RiseAttr { get; set; }
		/// <summary>MaxLevel</summary>
		[ProtoMember(7)]
		public int MaxLevel { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(8)]
		public string Des { get; set; }

	}
}
