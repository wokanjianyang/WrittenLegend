using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipQualityConfigCategory : ProtoObject, IMerge
    {
        public static EquipQualityConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipQualityConfig> dict = new Dictionary<int, EquipQualityConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipQualityConfig> list = new List<EquipQualityConfig>();
		
        public EquipQualityConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipQualityConfigCategory s = o as EquipQualityConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipQualityConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipQualityConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipQualityConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipQualityConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipQualityConfig> GetAll()
        {
            return this.dict;
        }

        public EquipQualityConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipQualityConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>RandomNum</summary>
		[ProtoMember(2)]
		public int RandomNum { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(3)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public int[] AttrValueList { get; set; }

	}
}
