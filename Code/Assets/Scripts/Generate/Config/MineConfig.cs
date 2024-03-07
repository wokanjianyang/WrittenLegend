using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class MineConfigCategory : ProtoObject, IMerge
    {
        public static MineConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MineConfig> dict = new Dictionary<int, MineConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MineConfig> list = new List<MineConfig>();
		
        public MineConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MineConfigCategory s = o as MineConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MineConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MineConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MineConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MineConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MineConfig> GetAll()
        {
            return this.dict;
        }

        public MineConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MineConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Rate</summary>
		[ProtoMember(4)]
		public int Rate { get; set; }
		/// <summary>StartRate</summary>
		[ProtoMember(5)]
		public int StartRate { get; set; }
		/// <summary>EndRate</summary>
		[ProtoMember(6)]
		public int EndRate { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(7)]
		public long AttrValue { get; set; }
		/// <summary>RiseAttr</summary>
		[ProtoMember(8)]
		public int RiseAttr { get; set; }
		/// <summary>RisePower</summary>
		[ProtoMember(9)]
		public int RisePower { get; set; }
		/// <summary>MaxLevel</summary>
		[ProtoMember(10)]
		public int MaxLevel { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(11)]
		public string Des { get; set; }

	}
}
