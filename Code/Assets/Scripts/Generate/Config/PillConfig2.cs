using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PillConfig2Category : ProtoObject, IMerge
    {
        public static PillConfig2Category Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PillConfig2> dict = new Dictionary<int, PillConfig2>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PillConfig2> list = new List<PillConfig2>();
		
        public PillConfig2Category()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PillConfig2Category s = o as PillConfig2Category;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PillConfig2 config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PillConfig2 Get(int id)
        {
            this.dict.TryGetValue(id, out PillConfig2 item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PillConfig2)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PillConfig2> GetAll()
        {
            return this.dict;
        }

        public PillConfig2 GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PillConfig2: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Position</summary>
		[ProtoMember(3)]
		public int Position { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public double AttrValue { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(6)]
		public int Fee { get; set; }

	}
}
