using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterPillConfigCategory : ProtoObject, IMerge
    {
        public static MonsterPillConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterPillConfig> dict = new Dictionary<int, MonsterPillConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterPillConfig> list = new List<MonsterPillConfig>();
		
        public MonsterPillConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterPillConfigCategory s = o as MonsterPillConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterPillConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterPillConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterPillConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterPillConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterPillConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterPillConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterPillConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(2)]
		public string MapName { get; set; }
		/// <summary>MonsterName</summary>
		[ProtoMember(3)]
		public string MonsterName { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(4)]
		public string Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(5)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(6)]
		public string HP { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(7)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(8)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(9)]
		public int CritRateResist { get; set; }
		/// <summary>ResotrePercent</summary>
		[ProtoMember(10)]
		public int ResotrePercent { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(11)]
		public int Miss { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(12)]
		public int Protect { get; set; }

	}
}
