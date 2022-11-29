using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;
using System.Linq;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class DropConfigCategory : ProtoObject, IMerge
    {
        public static DropConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, DropConfig> dict = new Dictionary<int, DropConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<DropConfig> list = new List<DropConfig>();
		
        public DropConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            DropConfigCategory s = o as DropConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (DropConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public DropConfig Get(int id)
        {
            this.dict.TryGetValue(id, out DropConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (DropConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, DropConfig> GetAll()
        {
            return this.dict;
        }

        public DropConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }

        public DropConfig GetByMonsterId(int monsterId) {
            return this.dict.Where(m => m.Value.MonsterID == monsterId).First().Value;
        }
    }

    [ProtoContract]
	public partial class DropConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>掉落ID</summary>
		[ProtoMember(2)]
		public int DropID { get; set; }
		/// <summary>怪物ID</summary>
		[ProtoMember(3)]
		public int MonsterID { get; set; }
		/// <summary>地图ID</summary>
		[ProtoMember(4)]
		public int MapID { get; set; }
		/// <summary>掉落信息</summary>
		[ProtoMember(5)]
		public string Info { get; set; }
		/// <summary>随机类型</summary>
		[ProtoMember(6)]
		public int RandomType { get; set; }
		/// <summary>掉落物</summary>
		[ProtoMember(7)]
		public int[] ItemList { get; set; }
		/// <summary>概率</summary>
		[ProtoMember(8)]
		public int[] Rate { get; set; }

	}
}
