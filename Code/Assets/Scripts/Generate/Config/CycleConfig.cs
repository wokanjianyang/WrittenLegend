using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class CycleConfigCategory : ProtoObject, IMerge
    {
        public static CycleConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CycleConfig> dict = new Dictionary<int, CycleConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CycleConfig> list = new List<CycleConfig>();
		
        public CycleConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            CycleConfigCategory s = o as CycleConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (CycleConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public CycleConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CycleConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CycleConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CycleConfig> GetAll()
        {
            return this.dict;
        }

        public CycleConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CycleConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(2)]
		public long Cycle { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(3)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public long[] AttrValueList { get; set; }

	}
}
