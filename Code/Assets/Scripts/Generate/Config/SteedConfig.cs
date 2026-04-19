using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SteedConfigCategory : ProtoObject, IMerge
    {
        public static SteedConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SteedConfig> dict = new Dictionary<int, SteedConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SteedConfig> list = new List<SteedConfig>();
		
        public SteedConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SteedConfigCategory s = o as SteedConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SteedConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SteedConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SteedConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SteedConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SteedConfig> GetAll()
        {
            return this.dict;
        }

        public SteedConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SteedConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(2)]
		public int Cycle { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(3)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(4)]
		public double AttrValue { get; set; }
		/// <summary>StartQuality</summary>
		[ProtoMember(5)]
		public int StartQuality { get; set; }
		/// <summary>EndQuality</summary>
		[ProtoMember(6)]
		public int EndQuality { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(7)]
		public int Role { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(8)]
		public string Desc { get; set; }

	}
}
