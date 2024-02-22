using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipRedConfigCategory : ProtoObject, IMerge
    {
        public static EquipRedConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipRedConfig> dict = new Dictionary<int, EquipRedConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipRedConfig> list = new List<EquipRedConfig>();
		
        public EquipRedConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipRedConfigCategory s = o as EquipRedConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipRedConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipRedConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipRedConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipRedConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipRedConfig> GetAll()
        {
            return this.dict;
        }

        public EquipRedConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipRedConfig: ProtoObject, IConfig
	{
		/// <summary>_Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Role</summary>
		[ProtoMember(2)]
		public int Role { get; set; }
		/// <summary>Count</summary>
		[ProtoMember(3)]
		public int Count { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(4)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public int AttrValue { get; set; }
		/// <summary>AttrRise</summary>
		[ProtoMember(6)]
		public int AttrRise { get; set; }

	}
}
