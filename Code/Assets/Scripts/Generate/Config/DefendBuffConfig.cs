using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class DefendBuffConfigCategory : ProtoObject, IMerge
    {
        public static DefendBuffConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DefendBuffConfig> dict = new Dictionary<int, DefendBuffConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DefendBuffConfig> list = new List<DefendBuffConfig>();
		
        public DefendBuffConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            DefendBuffConfigCategory s = o as DefendBuffConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (DefendBuffConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DefendBuffConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DefendBuffConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DefendBuffConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DefendBuffConfig> GetAll()
        {
            return this.dict;
        }

        public DefendBuffConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class DefendBuffConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>MaxCount</summary>
		[ProtoMember(4)]
		public int MaxCount { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(5)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(6)]
		public int AttrValue { get; set; }
		/// <summary>SkillId</summary>
		[ProtoMember(7)]
		public int SkillId { get; set; }
		/// <summary>RuneId</summary>
		[ProtoMember(8)]
		public int RuneId { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(9)]
		public string Memo { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(10)]
		public int Rate { get; set; }

	}
}
