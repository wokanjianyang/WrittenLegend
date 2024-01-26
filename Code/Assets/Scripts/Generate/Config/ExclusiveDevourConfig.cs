using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ExclusiveDevourConfigCategory : ProtoObject, IMerge
    {
        public static ExclusiveDevourConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExclusiveDevourConfig> dict = new Dictionary<int, ExclusiveDevourConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExclusiveDevourConfig> list = new List<ExclusiveDevourConfig>();
		
        public ExclusiveDevourConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExclusiveDevourConfigCategory s = o as ExclusiveDevourConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ExclusiveDevourConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExclusiveDevourConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExclusiveDevourConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExclusiveDevourConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExclusiveDevourConfig> GetAll()
        {
            return this.dict;
        }

        public ExclusiveDevourConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExclusiveDevourConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Level</summary>
		[ProtoMember(2)]
		public int Level { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(3)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemCountList</summary>
		[ProtoMember(4)]
		public int[] ItemCountList { get; set; }

	}
}
