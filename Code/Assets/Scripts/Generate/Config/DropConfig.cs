using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
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
    }

    [ProtoContract]
	public partial class DropConfig: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>等级</summary>
		[ProtoMember(2)]
		public int Level { get; set; }
		/// <summary>掉落名称</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>掉落数量</summary>
		[ProtoMember(4)]
		public int Quantity { get; set; }
		/// <summary>道具类型</summary>
		[ProtoMember(5)]
		public int ItemType { get; set; }
		/// <summary>掉落Id列表</summary>
		[ProtoMember(6)]
		public int[] ItemIdList { get; set; }

	}
}
