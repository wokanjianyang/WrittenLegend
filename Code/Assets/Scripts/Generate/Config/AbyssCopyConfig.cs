using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AbyssCopyConfigCategory : ProtoObject, IMerge
    {
        public static AbyssCopyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AbyssCopyConfig> dict = new Dictionary<int, AbyssCopyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AbyssCopyConfig> list = new List<AbyssCopyConfig>();
		
        public AbyssCopyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AbyssCopyConfigCategory s = o as AbyssCopyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AbyssCopyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AbyssCopyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AbyssCopyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AbyssCopyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AbyssCopyConfig> GetAll()
        {
            return this.dict;
        }

        public AbyssCopyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AbyssCopyConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Stage</summary>
		[ProtoMember(2)]
		public int Stage { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(3)]
		public int Cycle { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(4)]
		public string MapName { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(5)]
		public string Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(6)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(7)]
		public string HP { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(8)]
		public int Speed { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(9)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(10)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(11)]
		public int CritRateResist { get; set; }
		/// <summary>CritDamageResist</summary>
		[ProtoMember(12)]
		public int CritDamageResist { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(13)]
		public int Protect { get; set; }

	}
}
