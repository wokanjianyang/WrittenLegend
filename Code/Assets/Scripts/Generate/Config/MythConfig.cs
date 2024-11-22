using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MythConfigCategory : ProtoObject, IMerge
    {
        public static MythConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MythConfig> dict = new Dictionary<int, MythConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MythConfig> list = new List<MythConfig>();
		
        public MythConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MythConfigCategory s = o as MythConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MythConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MythConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MythConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MythConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MythConfig> GetAll()
        {
            return this.dict;
        }

        public MythConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MythConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapName</summary>
		[ProtoMember(2)]
		public string MapName { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(3)]
		public string Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(4)]
		public string Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(5)]
		public string HP { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(6)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(7)]
		public int DamageResist { get; set; }
		/// <summary>CritRateResist</summary>
		[ProtoMember(8)]
		public int CritRateResist { get; set; }
		/// <summary>ResotrePercent</summary>
		[ProtoMember(9)]
		public int ResotrePercent { get; set; }
		/// <summary>ItemType</summary>
		[ProtoMember(10)]
		public int[] ItemType { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(11)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemQuantity</summary>
		[ProtoMember(12)]
		public int[] ItemQuantity { get; set; }

	}
}
