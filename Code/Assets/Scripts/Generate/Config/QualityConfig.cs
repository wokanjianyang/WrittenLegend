using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class QualityConfigCategory : ProtoObject, IMerge
    {
        public static QualityConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, QualityConfig> dict = new Dictionary<int, QualityConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<QualityConfig> list = new List<QualityConfig>();
		
        public QualityConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            QualityConfigCategory s = o as QualityConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (QualityConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public QualityConfig Get(int id)
        {
            this.dict.TryGetValue(id, out QualityConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (QualityConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, QualityConfig> GetAll()
        {
            return this.dict;
        }

        public QualityConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class QualityConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>怪物称号</summary>
		[ProtoMember(2)]
		public string MonsterTitle { get; set; }
		/// <summary>攻击系数</summary>
		[ProtoMember(3)]
		public double AttrRate { get; set; }
		/// <summary>防御系数</summary>
		[ProtoMember(4)]
		public double DefRate { get; set; }
		/// <summary>血量系数</summary>
		[ProtoMember(5)]
		public double HpRate { get; set; }
		/// <summary>金币系数</summary>
		[ProtoMember(6)]
		public int GoldRate { get; set; }
		/// <summary>经验系数</summary>
		[ProtoMember(7)]
		public int ExpRate { get; set; }
		/// <summary>掉率系数</summary>
		[ProtoMember(8)]
		public int DropRate { get; set; }
		/// <summary>品质系数</summary>
		[ProtoMember(9)]
		public int QualityRate { get; set; }
		/// <summary>保底系数</summary>
		[ProtoMember(10)]
		public int CountRate { get; set; }

	}
}
