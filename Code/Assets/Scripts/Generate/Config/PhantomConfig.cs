using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PhantomConfigCategory : ProtoObject, IMerge
    {
        public static PhantomConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PhantomConfig> dict = new Dictionary<int, PhantomConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PhantomConfig> list = new List<PhantomConfig>();
		
        public PhantomConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PhantomConfigCategory s = o as PhantomConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PhantomConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PhantomConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PhantomConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PhantomConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PhantomConfig> GetAll()
        {
            return this.dict;
        }

        public PhantomConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PhantomConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }

	}
}
