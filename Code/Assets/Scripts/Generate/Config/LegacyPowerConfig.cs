using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class LegacyPowerConfigCategory : ProtoObject, IMerge
    {
        public static LegacyPowerConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, LegacyPowerConfig> dict = new Dictionary<int, LegacyPowerConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<LegacyPowerConfig> list = new List<LegacyPowerConfig>();
		
        public LegacyPowerConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            LegacyPowerConfigCategory s = o as LegacyPowerConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (LegacyPowerConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public LegacyPowerConfig Get(int id)
        {
            this.dict.TryGetValue(id, out LegacyPowerConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (LegacyPowerConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, LegacyPowerConfig> GetAll()
        {
            return this.dict;
        }

        public LegacyPowerConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class LegacyPowerConfig: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>MapId</summary>
		[ProtoMember(2)]
		public int MapId { get; set; }
		/// <summary>StartLevel</summary>
		[ProtoMember(3)]
		public int StartLevel { get; set; }
		/// <summary>EndLevel</summary>
		[ProtoMember(4)]
		public int EndLevel { get; set; }
		/// <summary>PowerList</summary>
		[ProtoMember(5)]
		public int[] PowerList { get; set; }
		/// <summary>PowerRiseList</summary>
		[ProtoMember(6)]
		public int[] PowerRiseList { get; set; }

	}
}
