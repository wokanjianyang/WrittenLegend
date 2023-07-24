using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipStrengthConfigCategory : ProtoObject, IMerge
    {
        public static EquipStrengthConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipStrengthConfig> dict = new Dictionary<int, EquipStrengthConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipStrengthConfig> list = new List<EquipStrengthConfig>();
		
        public EquipStrengthConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipStrengthConfigCategory s = o as EquipStrengthConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipStrengthConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipStrengthConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipStrengthConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipStrengthConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipStrengthConfig> GetAll()
        {
            return this.dict;
        }

        public EquipStrengthConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipStrengthConfig: ProtoObject, IConfig
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

	}
}
