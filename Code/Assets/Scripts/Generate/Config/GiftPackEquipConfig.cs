using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class GiftPackEquipConfigCategory : ProtoObject, IMerge
    {
        public static GiftPackEquipConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GiftPackEquipConfig> dict = new Dictionary<int, GiftPackEquipConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GiftPackEquipConfig> list = new List<GiftPackEquipConfig>();
		
        public GiftPackEquipConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GiftPackEquipConfigCategory s = o as GiftPackEquipConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (GiftPackEquipConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GiftPackEquipConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GiftPackEquipConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GiftPackEquipConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GiftPackEquipConfig> GetAll()
        {
            return this.dict;
        }

        public GiftPackEquipConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GiftPackEquipConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>EquipId</summary>
		[ProtoMember(2)]
		public int EquipId { get; set; }
		/// <summary>RuneId</summary>
		[ProtoMember(3)]
		public int RuneId { get; set; }
		/// <summary>SuitId</summary>
		[ProtoMember(4)]
		public int SuitId { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(5)]
		public int[] AttrIdList { get; set; }

	}
}
