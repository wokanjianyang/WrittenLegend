using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SpiritCopyConfigCategory : ProtoObject, IMerge
    {
        public static SpiritCopyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SpiritCopyConfig> dict = new Dictionary<int, SpiritCopyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SpiritCopyConfig> list = new List<SpiritCopyConfig>();
		
        public SpiritCopyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SpiritCopyConfigCategory s = o as SpiritCopyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SpiritCopyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SpiritCopyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SpiritCopyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SpiritCopyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SpiritCopyConfig> GetAll()
        {
            return this.dict;
        }

        public SpiritCopyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SpiritCopyConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(2)]
		public int Cycle { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(3)]
		public string MapName { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(4)]
		public string MonsterName { get; set; }
		/// <summary>Require</summary>
		[ProtoMember(5)]
		public int Require { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(6)]
		public string Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(7)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(8)]
		public string HP { get; set; }
		/// <summary>Speed</summary>
		[ProtoMember(9)]
		public int Speed { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(10)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(11)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(12)]
		public int CritRateResist { get; set; }
		/// <summary>CritDamageResist</summary>
		[ProtoMember(13)]
		public int CritDamageResist { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(14)]
		public int Protect { get; set; }

	}
}
