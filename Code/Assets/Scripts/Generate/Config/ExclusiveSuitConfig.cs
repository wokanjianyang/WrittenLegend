using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class ExclusiveSuitConfigCategory : ProtoObject, IMerge
    {
        public static ExclusiveSuitConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExclusiveSuitConfig> dict = new Dictionary<int, ExclusiveSuitConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExclusiveSuitConfig> list = new List<ExclusiveSuitConfig>();
		
        public ExclusiveSuitConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExclusiveSuitConfigCategory s = o as ExclusiveSuitConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ExclusiveSuitConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExclusiveSuitConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExclusiveSuitConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExclusiveSuitConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExclusiveSuitConfig> GetAll()
        {
            return this.dict;
        }

        public ExclusiveSuitConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExclusiveSuitConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>Name</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>Cycle</summary>
		[ProtoMember(3)]
		public int Cycle { get; set; }
		/// <summary>StartPart</summary>
		[ProtoMember(4)]
		public int StartPart { get; set; }
		/// <summary>EndPart</summary>
		[ProtoMember(5)]
		public int EndPart { get; set; }
		/// <summary>AttrId</summary>
		[ProtoMember(6)]
		public int AttrId { get; set; }
		/// <summary>AttrValue</summary>
		[ProtoMember(7)]
		public int AttrValue { get; set; }
		/// <summary>Desc</summary>
		[ProtoMember(8)]
		public string Desc { get; set; }

	}
}
