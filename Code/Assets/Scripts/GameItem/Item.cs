using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Item
    {
        public long Id { get; set; }

        public int ConfigId { get; set; }
        public string Name { get; set; }

        public string Des { get; set; }

        /// <summary>
        ///  道具类型
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 装备所需等级
        /// </summary>
        public int Level { get; set; }

        public int Gold { get; set; }

        /// <summary>
        /// 道具品质，用做显示或者其他
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// 堆叠数量
        /// </summary>
        public int MaxNum { get; set; }

        /// <summary>
        /// 属性列表
        /// </summary>
        public IDictionary<int, long> AttrList { get; set; }

        public IDictionary<int, SkillRune> skillList { get; set; }

        public int BoxId { get; set; } = -1;
    }
}
