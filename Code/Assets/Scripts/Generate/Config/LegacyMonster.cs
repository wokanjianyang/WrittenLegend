using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyMonsterCategory : ProtoObject, IMerge
    {
        public static LegacyMonsterCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyMonster> dict = new Dictionary<int, LegacyMonster>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyMonster> list = new List<LegacyMonster>();
		
        public LegacyMonsterCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyMonsterCategory s = o as LegacyMonsterCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyMonster config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyMonster Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyMonster item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyMonster)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyMonster> GetAll()
        {
            return this.dict;
        }

        public LegacyMonster GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyMonster: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(2)]
		public int Role { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(3)]
		public string Attr { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(4)]
		public string RiseAttr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(5)]
		public string Def { get; set; }
		/// <summary>RiseDef</summary>
		[ProtoMember(6)]
		public string RiseDef { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(7)]
		public string HP { get; set; }
		/// <summary>RiseHp</summary>
		[ProtoMember(8)]
		public string RiseHp { get; set; }
		/// <summary>SkillIdList</summary>
		[ProtoMember(9)]
		public int[] SkillIdList { get; set; }

	}
}
