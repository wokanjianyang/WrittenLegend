using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class StoneSetConfigCategory : ProtoObject, IMerge
    {
        public static StoneSetConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, StoneSetConfig> dict = new Dictionary<int, StoneSetConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<StoneSetConfig> list = new List<StoneSetConfig>();
		
        public StoneSetConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            StoneSetConfigCategory s = o as StoneSetConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (StoneSetConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public StoneSetConfig Get(int id)
        {
            this.dict.TryGetValue(id, out StoneSetConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (StoneSetConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, StoneSetConfig> GetAll()
        {
            return this.dict;
        }

        public StoneSetConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class StoneSetConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>TypeList</summary>
		[ProtoMember(3)]
		public int[] TypeList { get; set; }

	}
}
