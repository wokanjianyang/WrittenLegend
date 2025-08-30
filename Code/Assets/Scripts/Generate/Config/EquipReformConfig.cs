using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipReformConfigCategory : ProtoObject, IMerge
    {
        public static EquipReformConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipReformConfig> dict = new Dictionary<int, EquipReformConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipReformConfig> list = new List<EquipReformConfig>();
		
        public EquipReformConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipReformConfigCategory s = o as EquipReformConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipReformConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipReformConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipReformConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipReformConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipReformConfig> GetAll()
        {
            return this.dict;
        }

        public EquipReformConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipReformConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Position</summary>
		[ProtoMember(2)]
		public int Position { get; set; }
		/// <summary>AttrList</summary>
		[ProtoMember(3)]
		public int[] AttrList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public long[] AttrValueList { get; set; }
		/// <summary>RequireLevel</summary>
		[ProtoMember(5)]
		public int[] RequireLevel { get; set; }

	}
}
