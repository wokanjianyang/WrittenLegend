using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterBaseCategory : ProtoObject, IMerge
    {
        public static MonsterBaseCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterBase> dict = new Dictionary<int, MonsterBase>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterBase> list = new List<MonsterBase>();
		
        public MonsterBaseCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterBaseCategory s = o as MonsterBaseCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterBase config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterBase Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterBase item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterBase)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterBase> GetAll()
        {
            return this.dict;
        }

        public MonsterBase GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterBase: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>等级</summary>
		[ProtoMember(2)]
		public int Level { get; set; }
		/// <summary>名称</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>攻击</summary>
		[ProtoMember(4)]
		public long PhyAttr { get; set; }
		/// <summary>防御</summary>
		[ProtoMember(5)]
		public long Def { get; set; }
		/// <summary>生命</summary>
		[ProtoMember(6)]
		public long HP { get; set; }
		/// <summary>经验</summary>
		[ProtoMember(7)]
		public long Exp { get; set; }
		/// <summary>掉落金币</summary>
		[ProtoMember(8)]
		public long Gold { get; set; }

	}
}
