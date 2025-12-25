using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SpiritConfigCategory : ProtoObject, IMerge
    {
        public static SpiritConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SpiritConfig> dict = new Dictionary<int, SpiritConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SpiritConfig> list = new List<SpiritConfig>();
		
        public SpiritConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SpiritConfigCategory s = o as SpiritConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SpiritConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SpiritConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SpiritConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SpiritConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SpiritConfig> GetAll()
        {
            return this.dict;
        }

        public SpiritConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SpiritConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>Quality</summary>
		[ProtoMember(3)]
		public int Quality { get; set; }
		/// <summary>MaxLevel</summary>
		[ProtoMember(4)]
		public int MaxLevel { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(5)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(6)]
		public string Name { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(7)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(8)]
		public double[] AttrValueList { get; set; }

	}
}
