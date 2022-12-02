using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class GameAttributeCategory : ProtoObject, IMerge
    {
        public static GameAttributeCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GameAttribute> dict = new Dictionary<int, GameAttribute>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GameAttribute> list = new List<GameAttribute>();
		
        public GameAttributeCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GameAttributeCategory s = o as GameAttributeCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (GameAttribute config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GameAttribute Get(int id)
        {
            this.dict.TryGetValue(id, out GameAttribute item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GameAttribute)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GameAttribute> GetAll()
        {
            return this.dict;
        }

        public GameAttribute GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GameAttribute: ProtoObject, IConfig
	{
		/// <summary>_id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>类型</summary>
		[ProtoMember(2)]
		public int Type { get; set; }
		/// <summary>战斗力系数</summary>
		[ProtoMember(3)]
		public float PowerCoef { get; set; }
		/// <summary>说明</summary>
		[ProtoMember(4)]
		public string Name { get; set; }

	}
}
