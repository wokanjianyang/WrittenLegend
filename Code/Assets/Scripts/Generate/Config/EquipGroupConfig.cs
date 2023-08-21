using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class EquipGroupConfigCategory : ProtoObject, IMerge
    {
        public static EquipGroupConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, EquipGroupConfig> dict = new Dictionary<int, EquipGroupConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<EquipGroupConfig> list = new List<EquipGroupConfig>();
		
        public EquipGroupConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            EquipGroupConfigCategory s = o as EquipGroupConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (EquipGroupConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public EquipGroupConfig Get(int id)
        {
            this.dict.TryGetValue(id, out EquipGroupConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (EquipGroupConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, EquipGroupConfig> GetAll()
        {
            return this.dict;
        }

        public EquipGroupConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class EquipGroupConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Level</summary>
		[ProtoMember(2)]
		public int Level { get; set; }
		/// <summary>Position</summary>
		[ProtoMember(3)]
		public int Position { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(4)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(5)]
		public int[] AttrValueList { get; set; }
		/// <summary>Memo</summary>
		[ProtoMember(6)]
		public string Memo { get; set; }

	}
}
