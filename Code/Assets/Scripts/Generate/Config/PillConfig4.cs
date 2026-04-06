using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PillConfig4Category : ProtoObject, IMerge
    {
        public static PillConfig4Category Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PillConfig4> dict = new Dictionary<int, PillConfig4>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PillConfig4> list = new List<PillConfig4>();
		
        public PillConfig4Category()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PillConfig4Category s = o as PillConfig4Category;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PillConfig4 config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PillConfig4 Get(int id)
        {
            this.dict.TryGetValue(id, out PillConfig4 item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PillConfig4)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PillConfig4> GetAll()
        {
            return this.dict;
        }

        public PillConfig4 GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PillConfig4: ProtoObject, IConfig
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
