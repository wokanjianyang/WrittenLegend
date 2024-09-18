using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MapNewAttrCategory : ProtoObject, IMerge
    {
        public static MapNewAttrCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MapNewAttr> dict = new Dictionary<int, MapNewAttr>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MapNewAttr> list = new List<MapNewAttr>();
		
        public MapNewAttrCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MapNewAttrCategory s = o as MapNewAttrCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MapNewAttr config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MapNewAttr Get(int id)
        {
            this.dict.TryGetValue(id, out MapNewAttr item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MapNewAttr)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MapNewAttr> GetAll()
        {
            return this.dict;
        }

        public MapNewAttr GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MapNewAttr: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Layer</summary>
		[ProtoMember(2)]
		public int Layer { get; set; }
		/// <summary>进入等级要求</summary>
		[ProtoMember(3)]
		public string LevelRequired { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(4)]
		public string Memo { get; set; }
		/// <summary>攻击</summary>
		[ProtoMember(5)]
		public string Attr { get; set; }
		/// <summary>防御</summary>
		[ProtoMember(6)]
		public string Def { get; set; }
		/// <summary>生命</summary>
		[ProtoMember(7)]
		public string HP { get; set; }
		/// <summary>DamageIncrea</summary>
		[ProtoMember(8)]
		public int DamageIncrea { get; set; }
		/// <summary>DamageResist</summary>
		[ProtoMember(9)]
		public int DamageResist { get; set; }
		/// <summary>CritRate</summary>
		[ProtoMember(10)]
		public int CritRate { get; set; }
		/// <summary>CritDamage</summary>
		[ProtoMember(11)]
		public int CritDamage { get; set; }
		/// <summary>Accuracy</summary>
		[ProtoMember(12)]
		public int Accuracy { get; set; }
		/// <summary>Miss</summary>
		[ProtoMember(13)]
		public int Miss { get; set; }
		/// <summary>Protect</summary>
		[ProtoMember(14)]
		public int Protect { get; set; }
		/// <summary>经验</summary>
		[ProtoMember(15)]
		public long Exp { get; set; }
		/// <summary>掉落金币</summary>
		[ProtoMember(16)]
		public long Gold { get; set; }

	}
}
