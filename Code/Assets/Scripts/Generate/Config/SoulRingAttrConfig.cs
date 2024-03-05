using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SoulRingAttrConfigCategory : ProtoObject, IMerge
    {
        public static SoulRingAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SoulRingAttrConfig> dict = new Dictionary<int, SoulRingAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SoulRingAttrConfig> list = new List<SoulRingAttrConfig>();
		
        public SoulRingAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SoulRingAttrConfigCategory s = o as SoulRingAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SoulRingAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SoulRingAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SoulRingAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SoulRingAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SoulRingAttrConfig> GetAll()
        {
            return this.dict;
        }

        public SoulRingAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SoulRingAttrConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Sid</summary>
		[ProtoMember(2)]
		public int Sid { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(5)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(6)]
		public long[] AttrValueList { get; set; }
		/// <summary>AttrRiseList</summary>
		[ProtoMember(7)]
		public long[] AttrRiseList { get; set; }
		/// <summary>AurasId</summary>
		[ProtoMember(8)]
		public int AurasId { get; set; }
		/// <summary>AurasLevel</summary>
		[ProtoMember(9)]
		public int AurasLevel { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(10)]
		public int Fee { get; set; }
		/// <summary>RiseFee</summary>
		[ProtoMember(11)]
		public int RiseFee { get; set; }
		/// <summary>LockMemo</summary>
		[ProtoMember(12)]
		public string LockMemo { get; set; }
		/// <summary>AurasMemo</summary>
		[ProtoMember(13)]
		public string AurasMemo { get; set; }

	}
}
