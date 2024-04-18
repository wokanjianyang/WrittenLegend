using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ArtifactConfigCategory : ProtoObject, IMerge
    {
        public static ArtifactConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ArtifactConfig> dict = new Dictionary<int, ArtifactConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ArtifactConfig> list = new List<ArtifactConfig>();
		
        public ArtifactConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ArtifactConfigCategory s = o as ArtifactConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ArtifactConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ArtifactConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ArtifactConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ArtifactConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ArtifactConfig> GetAll()
        {
            return this.dict;
        }

        public ArtifactConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ArtifactConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>ItemId</summary>
		[ProtoMember(2)]
		public int ItemId { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(3)]
		public string Name { get; set; }
		/// <summary>Type</summary>
		[ProtoMember(4)]
		public int Type { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(5)]
		public int AttrValue { get; set; }
		/// <summary>MaxCount</summary>
		[ProtoMember(6)]
		public int MaxCount { get; set; }
		/// <summary>DropRate</summary>
		[ProtoMember(7)]
		public int DropRate { get; set; }
		/// <summary>Des</summary>
		[ProtoMember(8)]
		public string Des { get; set; }

	}
}
