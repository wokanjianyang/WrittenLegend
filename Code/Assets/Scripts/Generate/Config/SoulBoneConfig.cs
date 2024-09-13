using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class SoulBoneConfigCategory : ProtoObject, IMerge
    {
        public static SoulBoneConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SoulBoneConfig> dict = new Dictionary<int, SoulBoneConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SoulBoneConfig> list = new List<SoulBoneConfig>();
		
        public SoulBoneConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SoulBoneConfigCategory s = o as SoulBoneConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SoulBoneConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SoulBoneConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SoulBoneConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SoulBoneConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SoulBoneConfig> GetAll()
        {
            return this.dict;
        }

        public SoulBoneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SoulBoneConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>AttrIdList</summary>
		[ProtoMember(3)]
		public int[] AttrIdList { get; set; }
		/// <summary>AttrValueList</summary>
		[ProtoMember(4)]
		public double[] AttrValueList { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(5)]
		public int ItemId { get; set; }

	}
}
