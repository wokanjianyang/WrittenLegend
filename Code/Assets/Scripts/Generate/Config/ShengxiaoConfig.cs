using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ShengxiaoConfigCategory : ProtoObject, IMerge
    {
        public static ShengxiaoConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ShengxiaoConfig> dict = new Dictionary<int, ShengxiaoConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ShengxiaoConfig> list = new List<ShengxiaoConfig>();
		
        public ShengxiaoConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ShengxiaoConfigCategory s = o as ShengxiaoConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ShengxiaoConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ShengxiaoConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ShengxiaoConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ShengxiaoConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ShengxiaoConfig> GetAll()
        {
            return this.dict;
        }

        public ShengxiaoConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ShengxiaoConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Part</summary>
		[ProtoMember(3)]
		public int Part { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(4)]
		public int Cycle { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(5)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(6)]
		public int[] AttrValueList { get; set; }
		/// <summary>AttchValueList</summary>
		[ProtoMember(7)]
		public int[] AttchValueList { get; set; }
		/// <summary>LayerValueList</summary>
		[ProtoMember(8)]
		public int[] LayerValueList { get; set; }

	}
}
