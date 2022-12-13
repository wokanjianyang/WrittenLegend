using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Equip : Item
    {
        public int Position { get; set; }

        /// <summary>
        /// 词条属性列表
        /// </summary>
        public List<KeyValuePair<int, long>> AttrEntryList { get; set; }

        /// <summary>
        /// 基础属性
        /// </summary>
        public IDictionary<int, long> BaseAttrList { get; set; }

        public SkillRune SkillRune { get; set; }
    }
}
