using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipStrengthFeeConfigCategory : ProtoObject, IMerge
    {
        public static EquipStrengthFeeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipStrengthFeeConfig> dict = new Dictionary<int, EquipStrengthFeeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipStrengthFeeConfig> list = new List<EquipStrengthFeeConfig>();
		
        public EquipStrengthFeeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipStrengthFeeConfigCategory s = o as EquipStrengthFeeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipStrengthFeeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipStrengthFeeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipStrengthFeeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipStrengthFeeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipStrengthFeeConfig> GetAll()
        {
            return this.dict;
        }

        public EquipStrengthFeeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipStrengthFeeConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public long StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(3)]
		public long EndLevel { get; set; }
		/// <summary>Fee</summary>
		[ProtoMember(4)]
		public long Fee { get; set; }

	}
}
