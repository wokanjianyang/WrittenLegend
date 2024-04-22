using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class InfiniteModelConfigCategory : ProtoObject, IMerge
    {
        public static InfiniteModelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, InfiniteModelConfig> dict = new Dictionary<int, InfiniteModelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<InfiniteModelConfig> list = new List<InfiniteModelConfig>();
		
        public InfiniteModelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            InfiniteModelConfigCategory s = o as InfiniteModelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (InfiniteModelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public InfiniteModelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out InfiniteModelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (InfiniteModelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, InfiniteModelConfig> GetAll()
        {
            return this.dict;
        }

        public InfiniteModelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class InfiniteModelConfig: ProtoObject, IConfig
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

	}
}
