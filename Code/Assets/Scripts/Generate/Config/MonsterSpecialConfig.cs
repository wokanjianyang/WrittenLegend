using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MonsterSpecialConfigCategory : ProtoObject, IMerge
    {
        public static MonsterSpecialConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MonsterSpecialConfig> dict = new Dictionary<int, MonsterSpecialConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MonsterSpecialConfig> list = new List<MonsterSpecialConfig>();
		
        public MonsterSpecialConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MonsterSpecialConfigCategory s = o as MonsterSpecialConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MonsterSpecialConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MonsterSpecialConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MonsterSpecialConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MonsterSpecialConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MonsterSpecialConfig> GetAll()
        {
            return this.dict;
        }

        public MonsterSpecialConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MonsterSpecialConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapLevel</summary>
		[ProtoMember(2)]
		public int MapLevel { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Attr</summary>
		[ProtoMember(4)]
		public long Attr { get; set; }
		/// <summary>Def</summary>
		[ProtoMember(5)]
		public long Def { get; set; }
		/// <summary>HP</summary>
		[ProtoMember(6)]
		public long HP { get; set; }
		/// <summary>DropIdList</summary>
		[ProtoMember(7)]
		public int[] DropIdList { get; set; }
		/// <summary>DropRateList</summary>
		[ProtoMember(8)]
		public int[] DropRateList { get; set; }
		/// <summary>BuildRate</summary>
		[ProtoMember(9)]
		public int BuildRate { get; set; }

	}
}
