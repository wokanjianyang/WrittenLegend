using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AttrEntryConfigCategory : ProtoObject, IMerge
    {
        public static AttrEntryConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AttrEntryConfig> dict = new Dictionary<int, AttrEntryConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AttrEntryConfig> list = new List<AttrEntryConfig>();
		
        public AttrEntryConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AttrEntryConfigCategory s = o as AttrEntryConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AttrEntryConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AttrEntryConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AttrEntryConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AttrEntryConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AttrEntryConfig> GetAll()
        {
            return this.dict;
        }

        public AttrEntryConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AttrEntryConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(2)]
		public int AttrId { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(4)]
		public string Desc { get; set; }
		/// <summary>MinValue</summary>
		[ProtoMember(5)]
		public int MinValue { get; set; }
		/// <summary>MaxValue</summary>
		[ProtoMember(6)]
		public int MaxValue { get; set; }
		/// <summary>PartList</summary>
		[ProtoMember(7)]
		public int[] PartList { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(8)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(9)]
		public int EndLevel { get; set; }
		/// <summary>StartQuality</summary>
		[ProtoMember(10)]
		public int StartQuality { get; set; }
		/// <summary>EndQuality</summary>
		[ProtoMember(11)]
		public int EndQuality { get; set; }
		/// <summary>MaxCount</summary>
		[ProtoMember(12)]
		public int MaxCount { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(13)]
		public int Role { get; set; }

	}
}
