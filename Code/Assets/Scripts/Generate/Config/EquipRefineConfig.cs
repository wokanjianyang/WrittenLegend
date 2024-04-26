using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipRefineConfigCategory : ProtoObject, IMerge
    {
        public static EquipRefineConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipRefineConfig> dict = new Dictionary<int, EquipRefineConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipRefineConfig> list = new List<EquipRefineConfig>();
		
        public EquipRefineConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipRefineConfigCategory s = o as EquipRefineConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipRefineConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipRefineConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipRefineConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipRefineConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipRefineConfig> GetAll()
        {
            return this.dict;
        }

        public EquipRefineConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipRefineConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(2)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(3)]
		public int EndLevel { get; set; }
		/// <summary>BaseFee</summary>
		[ProtoMember(4)]
		public long BaseFee { get; set; }
		/// <summary>RiseFee</summary>
		[ProtoMember(5)]
		public long RiseFee { get; set; }

	}
}
