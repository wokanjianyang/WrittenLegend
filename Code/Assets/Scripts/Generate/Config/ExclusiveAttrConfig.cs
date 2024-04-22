using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ExclusiveAttrConfigCategory : ProtoObject, IMerge
    {
        public static ExclusiveAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExclusiveAttrConfig> dict = new Dictionary<int, ExclusiveAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExclusiveAttrConfig> list = new List<ExclusiveAttrConfig>();
		
        public ExclusiveAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExclusiveAttrConfigCategory s = o as ExclusiveAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ExclusiveAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExclusiveAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExclusiveAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExclusiveAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExclusiveAttrConfig> GetAll()
        {
            return this.dict;
        }

        public ExclusiveAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExclusiveAttrConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Level</summary>
		[ProtoMember(2)]
		public int Level { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(3)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public int[] AttrValueList { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(5)]
		public string Name { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(6)]
		public int Part { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(7)]
		public int Type { get; set; }
		/// <summary>品质</summary>
		[ProtoMember(8)]
		public int Quality { get; set; }
		/// <summary>词条</summary>
		[ProtoMember(9)]
		public int RuneId { get; set; }
		/// <summary>套装</summary>
		[ProtoMember(10)]
		public int SuitId { get; set; }

	}
}
