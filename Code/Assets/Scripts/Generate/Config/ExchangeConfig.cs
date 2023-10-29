using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ExchangeConfigCategory : ProtoObject, IMerge
    {
        public static ExchangeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExchangeConfig> dict = new Dictionary<int, ExchangeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExchangeConfig> list = new List<ExchangeConfig>();
		
        public ExchangeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExchangeConfigCategory s = o as ExchangeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ExchangeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExchangeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExchangeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExchangeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExchangeConfig> GetAll()
        {
            return this.dict;
        }

        public ExchangeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExchangeConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemTypeList</summary>
		[ProtoMember(2)]
		public int[] ItemTypeList { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(3)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemCountList</summary>
		[ProtoMember(4)]
		public int[] ItemCountList { get; set; }
		/// <summary>ItemQualityList</summary>
		[ProtoMember(5)]
		public int[] ItemQualityList { get; set; }
		/// <summary>TargetName</summary>
		[ProtoMember(6)]
		public string TargetName { get; set; }
		/// <summary>TargetType</summary>
		[ProtoMember(7)]
		public int TargetType { get; set; }
		/// <summary>TargetId</summary>
		[ProtoMember(8)]
		public int TargetId { get; set; }

	}
}
