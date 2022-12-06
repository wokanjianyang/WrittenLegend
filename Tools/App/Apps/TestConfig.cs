using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace Game
{
    [ProtoContract]
    [Config]
    public partial class TestConfigCategory : ProtoObject, IMerge
    {
        public static TestConfigCategory Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, TestConfig> dict = new Dictionary<int, TestConfig>();

        [BsonElement]
        [ProtoMember(1)]
        private List<TestConfig> list = new List<TestConfig>();

        public TestConfigCategory()
        {
            Instance = this;
        }

        public void Merge(object o)
        {
            TestConfigCategory s = o as TestConfigCategory;
            this.list.AddRange(s.list);
        }

        public override void EndInit()
        {
            foreach (TestConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }
            this.AfterEndInit();
        }

        public TestConfig Get(int id)
        {
            this.dict.TryGetValue(id, out TestConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(TestConfig)}，配置id: {id}");
            }

            return item;
        }

        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, TestConfig> GetAll()
        {
            return this.dict;
        }

        public TestConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
    public partial class TestConfig : ProtoObject, IConfig
    {
        /// <summary>Id</summary>
        [ProtoMember(1)]
        public int Id { get; set; }
        /// <summary>等级</summary>
        [ProtoMember(2)]
        public long Level { get; set; }
        /// <summary>攻击</summary>
        [ProtoMember(3)]
        public long PhyAtt { get; set; }
        /// <summary>防御</summary>
        [ProtoMember(4)]
        public long Def { get; set; }
        /// <summary>血量</summary>
        [ProtoMember(5)]
        public long Hp { get; set; }

    }
}
