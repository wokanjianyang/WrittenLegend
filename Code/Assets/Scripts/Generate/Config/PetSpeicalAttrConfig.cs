using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class PetSpeicalAttrConfigCategory : ProtoObject, IMerge
    {
        public static PetSpeicalAttrConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PetSpeicalAttrConfig> dict = new Dictionary<int, PetSpeicalAttrConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PetSpeicalAttrConfig> list = new List<PetSpeicalAttrConfig>();
		
        public PetSpeicalAttrConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PetSpeicalAttrConfigCategory s = o as PetSpeicalAttrConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PetSpeicalAttrConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PetSpeicalAttrConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PetSpeicalAttrConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PetSpeicalAttrConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PetSpeicalAttrConfig> GetAll()
        {
            return this.dict;
        }

        public PetSpeicalAttrConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PetSpeicalAttrConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLayer</summary>
		[ProtoMember(2)]
		public int StartLayer { get; set; }
		/// <summary>PetId</summary>
		[ProtoMember(3)]
		public int PetId { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public double AttrValue { get; set; }

	}
}
