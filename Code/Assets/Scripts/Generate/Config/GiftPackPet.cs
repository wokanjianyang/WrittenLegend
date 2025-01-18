using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class GiftPackPetCategory : ProtoObject, IMerge
    {
        public static GiftPackPetCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GiftPackPet> dict = new Dictionary<int, GiftPackPet>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GiftPackPet> list = new List<GiftPackPet>();
		
        public GiftPackPetCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GiftPackPetCategory s = o as GiftPackPetCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (GiftPackPet config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GiftPackPet Get(int id)
        {
            this.dict.TryGetValue(id, out GiftPackPet item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GiftPackPet)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GiftPackPet> GetAll()
        {
            return this.dict;
        }

        public GiftPackPet GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GiftPackPet: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(3)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public int[] AttrValueList { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(5)]
		public int Role { get; set; }

	}
}
