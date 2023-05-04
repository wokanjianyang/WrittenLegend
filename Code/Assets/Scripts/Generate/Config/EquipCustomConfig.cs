using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipCustomConfigCategory : ProtoObject, IMerge
    {
        public static EquipCustomConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipCustomConfig> dict = new Dictionary<int, EquipCustomConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipCustomConfig> list = new List<EquipCustomConfig>();
		
        public EquipCustomConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipCustomConfigCategory s = o as EquipCustomConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipCustomConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipCustomConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipCustomConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipCustomConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipCustomConfig> GetAll()
        {
            return this.dict;
        }

        public EquipCustomConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipCustomConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>LevelRequired</summary>
		[ProtoMember(2)]
		public int LevelRequired { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(4)]
		public int Part { get; set; }
		/// <summary>Position</summary>
		[ProtoMember(5)]
		public int[] Position { get; set; }
		/// <summary>基础属性列表</summary>
		[ProtoMember(6)]
		public int[] BaseArray { get; set; }
		/// <summary>基础属性值</summary>
		[ProtoMember(7)]
		public long[] AttributeBase { get; set; }
		/// <summary>词条</summary>
		[ProtoMember(8)]
		public int RuneId { get; set; }
		/// <summary>套装</summary>
		[ProtoMember(9)]
		public int SuitId { get; set; }
		/// <summary>Price</summary>
		[ProtoMember(10)]
		public long Price { get; set; }

	}
}
