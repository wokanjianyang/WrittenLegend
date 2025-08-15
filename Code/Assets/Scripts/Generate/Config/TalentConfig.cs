using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class TalentConfigCategory : ProtoObject, IMerge
    {
        public static TalentConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, TalentConfig> dict = new Dictionary<int, TalentConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<TalentConfig> list = new List<TalentConfig>();
		
        public TalentConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            TalentConfigCategory s = o as TalentConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (TalentConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public TalentConfig Get(int id)
        {
            this.dict.TryGetValue(id, out TalentConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (TalentConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, TalentConfig> GetAll()
        {
            return this.dict;
        }

        public TalentConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class TalentConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(2)]
		public int Cycle { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Logo</summary>
		[ProtoMember(4)]
		public string Logo { get; set; }
		/// <summary>RequireId</summary>
		[ProtoMember(5)]
		public int RequireId { get; set; }
		/// <summary>RequireLevel</summary>
		[ProtoMember(6)]
		public int RequireLevel { get; set; }
		/// <summary>MaxLevel</summary>
		[ProtoMember(7)]
		public int MaxLevel { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(8)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(9)]
		public int AttrValue { get; set; }
		/// <summary>RiseType</summary>
		[ProtoMember(10)]
		public int RiseType { get; set; }
		/// <summary>RiseValue</summary>
		[ProtoMember(11)]
		public int RiseValue { get; set; }
		/// <summary>RiseUnit</summary>
		[ProtoMember(12)]
		public string RiseUnit { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(13)]
		public int Fee { get; set; }
		/// <summary>desc</summary>
		[ProtoMember(14)]
		public string desc { get; set; }

	}
}
