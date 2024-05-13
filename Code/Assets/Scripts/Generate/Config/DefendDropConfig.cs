using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class DefendDropConfigCategory : ProtoObject, IMerge
    {
        public static DefendDropConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DefendDropConfig> dict = new Dictionary<int, DefendDropConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DefendDropConfig> list = new List<DefendDropConfig>();
		
        public DefendDropConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            DefendDropConfigCategory s = o as DefendDropConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (DefendDropConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DefendDropConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DefendDropConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DefendDropConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DefendDropConfig> GetAll()
        {
            return this.dict;
        }

        public DefendDropConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DefendDropConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(2)]
		public int Layer { get; set; }
		/// <summary>DropId</summary>
		[ProtoMember(3)]
		public int DropId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(5)]
		public int Rate { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(6)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(7)]
		public int EndLevel { get; set; }
		/// <summary>RateLevel</summary>
		[ProtoMember(8)]
		public int RateLevel { get; set; }
		/// <summary>Max</summary>
		[ProtoMember(9)]
		public int Max { get; set; }

	}
}
