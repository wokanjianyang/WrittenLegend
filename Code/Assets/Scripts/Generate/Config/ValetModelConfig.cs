using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ValetModelConfigCategory : ProtoObject, IMerge
    {
        public static ValetModelConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ValetModelConfig> dict = new Dictionary<int, ValetModelConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ValetModelConfig> list = new List<ValetModelConfig>();
		
        public ValetModelConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ValetModelConfigCategory s = o as ValetModelConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ValetModelConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ValetModelConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ValetModelConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ValetModelConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ValetModelConfig> GetAll()
        {
            return this.dict;
        }

        public ValetModelConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ValetModelConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>FromSkillId</summary>
		[ProtoMember(2)]
		public int FromSkillId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>ModelType</summary>
		[ProtoMember(4)]
		public int ModelType { get; set; }
		/// <summary>SpeedRate</summary>
		[ProtoMember(5)]
		public int SpeedRate { get; set; }
		/// <summary>HpRate</summary>
		[ProtoMember(6)]
		public int HpRate { get; set; }
		/// <summary>AttrRate</summary>
		[ProtoMember(7)]
		public int AttrRate { get; set; }
		/// <summary>DefRate</summary>
		[ProtoMember(8)]
		public int DefRate { get; set; }
		/// <summary>RestorePercent</summary>
		[ProtoMember(9)]
		public int RestorePercent { get; set; }
		/// <summary>AdvanceRate</summary>
		[ProtoMember(10)]
		public int AdvanceRate { get; set; }
		/// <summary>SkillList</summary>
		[ProtoMember(11)]
		public int[] SkillList { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(12)]
		public string Desc { get; set; }
		/// <summary>AttrList</summary>
		[ProtoMember(13)]
		public int[] AttrList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(14)]
		public long[] AttrValueList { get; set; }

	}
}
