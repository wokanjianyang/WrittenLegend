using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipHoneConfigCategory : ProtoObject, IMerge
    {
        public static EquipHoneConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipHoneConfig> dict = new Dictionary<int, EquipHoneConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipHoneConfig> list = new List<EquipHoneConfig>();
		
        public EquipHoneConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipHoneConfigCategory s = o as EquipHoneConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipHoneConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipHoneConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipHoneConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipHoneConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipHoneConfig> GetAll()
        {
            return this.dict;
        }

        public EquipHoneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipHoneConfig: ProtoObject, IConfig
	{
		/// <summary>_ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(2)]
		public int AttrId { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(3)]
		public int Type { get; set; }
		/// <summary>描述</summary>
		[ProtoMember(4)]
		public string Desc { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public int AttrValue { get; set; }
		/// <summary>StartValue</summary>
		[ProtoMember(6)]
		public int StartValue { get; set; }

	}
}
