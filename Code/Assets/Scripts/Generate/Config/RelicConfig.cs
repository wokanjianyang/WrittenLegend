using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class RelicConfigCategory : ProtoObject, IMerge
    {
        public static RelicConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RelicConfig> dict = new Dictionary<int, RelicConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RelicConfig> list = new List<RelicConfig>();
		
        public RelicConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            RelicConfigCategory s = o as RelicConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (RelicConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RelicConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RelicConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RelicConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RelicConfig> GetAll()
        {
            return this.dict;
        }

        public RelicConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RelicConfig: ProtoObject, IConfig
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
		public double[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(7)]
		public double[] AttrRiseList { get; set; }

	}
}
