using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PlayerModelCategory : ProtoObject, IMerge
    {
        public static PlayerModelCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PlayerModel> dict = new Dictionary<int, PlayerModel>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PlayerModel> list = new List<PlayerModel>();
		
        public PlayerModelCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PlayerModelCategory s = o as PlayerModelCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PlayerModel config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PlayerModel Get(int id)
        {
            this.dict.TryGetValue(id, out PlayerModel item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PlayerModel)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PlayerModel> GetAll()
        {
            return this.dict;
        }

        public PlayerModel GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PlayerModel: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(3)]
		public int Layer { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(4)]
		public int MapId { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(5)]
		public int Quality { get; set; }
		/// <summary>AttrList</summary>
		[ProtoMember(6)]
		public int[] AttrList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(7)]
		public long[] AttrValueList { get; set; }
		/// <summary>SkillList</summary>
		[ProtoMember(8)]
		public int[] SkillList { get; set; }
		/// <summary>Rune</summary>
		[ProtoMember(9)]
		public int Rune { get; set; }
		/// <summary>Suit</summary>
		[ProtoMember(10)]
		public int Suit { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(11)]
		public string Desc { get; set; }

	}
}
