using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class HeroPhatomConfigCategory : ProtoObject, IMerge
    {
        public static HeroPhatomConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, HeroPhatomConfig> dict = new Dictionary<int, HeroPhatomConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<HeroPhatomConfig> list = new List<HeroPhatomConfig>();
		
        public HeroPhatomConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            HeroPhatomConfigCategory s = o as HeroPhatomConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (HeroPhatomConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public HeroPhatomConfig Get(int id)
        {
            this.dict.TryGetValue(id, out HeroPhatomConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (HeroPhatomConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, HeroPhatomConfig> GetAll()
        {
            return this.dict;
        }

        public HeroPhatomConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class HeroPhatomConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>SkillList</summary>
		[ProtoMember(3)]
		public int[] SkillList { get; set; }

	}
}
