using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ExclusiveConfigCategory : ProtoObject, IMerge
    {
        public static ExclusiveConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExclusiveConfig> dict = new Dictionary<int, ExclusiveConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExclusiveConfig> list = new List<ExclusiveConfig>();
		
        public ExclusiveConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExclusiveConfigCategory s = o as ExclusiveConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ExclusiveConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExclusiveConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExclusiveConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExclusiveConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExclusiveConfig> GetAll()
        {
            return this.dict;
        }

        public ExclusiveConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExclusiveConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(3)]
		public int Part { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(4)]
		public int Type { get; set; }
		/// <summary>Level</summary>
		[ProtoMember(5)]
		public int Level { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(6)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(7)]
		public int[] AttrValueList { get; set; }

	}
}
