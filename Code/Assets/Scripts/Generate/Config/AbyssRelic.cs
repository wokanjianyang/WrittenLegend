using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class AbyssRelicCategory : ProtoObject, IMerge
    {
        public static AbyssRelicCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, AbyssRelic> dict = new Dictionary<int, AbyssRelic>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<AbyssRelic> list = new List<AbyssRelic>();
		
        public AbyssRelicCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            AbyssRelicCategory s = o as AbyssRelicCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (AbyssRelic config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public AbyssRelic Get(int id)
        {
            this.dict.TryGetValue(id, out AbyssRelic item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (AbyssRelic)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, AbyssRelic> GetAll()
        {
            return this.dict;
        }

        public AbyssRelic GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class AbyssRelic: ProtoObject, IConfig
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
		public int[] AtrVueList { get; set; }

	}
}
