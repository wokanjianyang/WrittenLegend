using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class BossModelCategory : ProtoObject, IMerge
    {
        public static BossModelCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, BossModel> dict = new Dictionary<int, BossModel>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<BossModel> list = new List<BossModel>();
		
        public BossModelCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            BossModelCategory s = o as BossModelCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (BossModel config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public BossModel Get(int id)
        {
            this.dict.TryGetValue(id, out BossModel item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (BossModel)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, BossModel> GetAll()
        {
            return this.dict;
        }

        public BossModel GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class BossModel: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrList</summary>
		[ProtoMember(3)]
		public int[] AttrList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public long[] AttrValueList { get; set; }
		/// <summary>SkillList</summary>
		[ProtoMember(5)]
		public int[] SkillList { get; set; }
		/// <summary>Rune</summary>
		[ProtoMember(6)]
		public int Rune { get; set; }
		/// <summary>Suit</summary>
		[ProtoMember(7)]
		public int Suit { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(8)]
		public string Desc { get; set; }

	}
}
