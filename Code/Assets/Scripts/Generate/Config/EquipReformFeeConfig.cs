using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipReformFeeConfigCategory : ProtoObject, IMerge
    {
        public static EquipReformFeeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipReformFeeConfig> dict = new Dictionary<int, EquipReformFeeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipReformFeeConfig> list = new List<EquipReformFeeConfig>();
		
        public EquipReformFeeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipReformFeeConfigCategory s = o as EquipReformFeeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipReformFeeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipReformFeeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipReformFeeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipReformFeeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipReformFeeConfig> GetAll()
        {
            return this.dict;
        }

        public EquipReformFeeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipReformFeeConfig: ProtoObject, IConfig
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
		/// <summary>BaseFee</summary>
		[ProtoMember(4)]
		public double BaseFee { get; set; }
		/// <summary>RiseFee</summary>
		[ProtoMember(5)]
		public double RiseFee { get; set; }
		/// <summary>StoneFee</summary>
		[ProtoMember(6)]
		public int StoneFee { get; set; }

	}
}
