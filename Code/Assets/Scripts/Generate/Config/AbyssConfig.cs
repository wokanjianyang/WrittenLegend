using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AbyssConfigCategory : ProtoObject, IMerge
    {
        public static AbyssConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AbyssConfig> dict = new Dictionary<int, AbyssConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AbyssConfig> list = new List<AbyssConfig>();
		
        public AbyssConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AbyssConfigCategory s = o as AbyssConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AbyssConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AbyssConfig Get(int id)
        {
            this.dict.TryGetValue(id, out AbyssConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AbyssConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AbyssConfig> GetAll()
        {
            return this.dict;
        }

        public AbyssConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AbyssConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(2)]
		public int ItemId { get; set; }
		/// <summary>SetId</summary>
		[ProtoMember(3)]
		public int SetId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(4)]
		public string Name { get; set; }
		/// <summary>AtrIdList</summary>
		[ProtoMember(5)]
		public int[] AtrIdList { get; set; }
		/// <summary>AtrVueList</summary>
		[ProtoMember(6)]
		public double[] AtrVueList { get; set; }

	}
}
