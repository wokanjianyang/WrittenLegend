using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class GiftPackConfigCategory : ProtoObject, IMerge
    {
        public static GiftPackConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GiftPackConfig> dict = new Dictionary<int, GiftPackConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GiftPackConfig> list = new List<GiftPackConfig>();
		
        public GiftPackConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GiftPackConfigCategory s = o as GiftPackConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (GiftPackConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GiftPackConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GiftPackConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GiftPackConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GiftPackConfig> GetAll()
        {
            return this.dict;
        }

        public GiftPackConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GiftPackConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>GiftType</summary>
		[ProtoMember(3)]
		public int GiftType { get; set; }
		/// <summary>LevelRequired</summary>
		[ProtoMember(4)]
		public int LevelRequired { get; set; }
		/// <summary>ItemTypeList</summary>
		[ProtoMember(5)]
		public int[] ItemTypeList { get; set; }
		/// <summary>ItemIdList</summary>
		[ProtoMember(6)]
		public int[] ItemIdList { get; set; }
		/// <summary>ItemCountList</summary>
		[ProtoMember(7)]
		public int[] ItemCountList { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(8)]
		public string Des { get; set; }

	}
}
