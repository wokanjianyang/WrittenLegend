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
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(3)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(4)]
		public int AttrValue { get; set; }
		/// <summary>LevelIncrea</summary>
		[ProtoMember(5)]
		public int LevelIncrea { get; set; }
		/// <summary>MaxLevel</summary>
		[ProtoMember(6)]
		public int MaxLevel { get; set; }
		/// <summary>StoneNumber</summary>
		[ProtoMember(7)]
		public int StoneNumber { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(8)]
		public int Quality { get; set; }
		/// <summary>RiseLevel</summary>
		[ProtoMember(9)]
		public int RiseLevel { get; set; }
		/// <summary>RiseNumber</summary>
		[ProtoMember(10)]
		public int RiseNumber { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(11)]
		public string Des { get; set; }

	}
}
