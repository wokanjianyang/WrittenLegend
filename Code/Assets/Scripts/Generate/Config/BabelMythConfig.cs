using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class BabelMythConfigCategory : ProtoObject, IMerge
    {
        public static BabelMythConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BabelMythConfig> dict = new Dictionary<int, BabelMythConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BabelMythConfig> list = new List<BabelMythConfig>();
		
        public BabelMythConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            BabelMythConfigCategory s = o as BabelMythConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (BabelMythConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BabelMythConfig Get(int id)
        {
            this.dict.TryGetValue(id, out BabelMythConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BabelMythConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BabelMythConfig> GetAll()
        {
            return this.dict;
        }

        public BabelMythConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BabelMythConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Start</summary>
		[ProtoMember(2)]
		public int Start { get; set; }
		/// <summary>End</summary>
		[ProtoMember(3)]
		public int End { get; set; }
		/// <summary>ItemTypeList</summary>
		[ProtoMember(4)]
		public int[] ItemTypeList { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(5)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemCountList</summary>
		[ProtoMember(6)]
		public int[] ItemCountList { get; set; }
		/// <summary>ItemRateList</summary>
		[ProtoMember(7)]
		public int[] ItemRateList { get; set; }

	}
}
