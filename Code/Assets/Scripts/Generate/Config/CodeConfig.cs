using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class CodeConfigCategory : ProtoObject, IMerge
    {
        public static CodeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, CodeConfig> dict = new Dictionary<int, CodeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<CodeConfig> list = new List<CodeConfig>();
		
        public CodeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            CodeConfigCategory s = o as CodeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (CodeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public CodeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out CodeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (CodeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, CodeConfig> GetAll()
        {
            return this.dict;
        }

        public CodeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class CodeConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>code</summary>
		[ProtoMember(3)]
		public string code { get; set; }
		/// <summary>物品类型</summary>
		[ProtoMember(4)]
		public int[] ItemTypeList { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(5)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemQuanlityList</summary>
		[ProtoMember(6)]
		public int[] ItemQuanlityList { get; set; }

	}
}
