using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Equip : Item
    {
        public Equip() {
            this.Type = ItemType.Equip;
        }
        public int Position { get; set; }

        /// <summary>
        /// ���������б�
        /// </summary>
        public List<KeyValuePair<int, long>> AttrEntryList { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public IDictionary<int, long> BaseAttrList { get; set; }

        public SkillRune SkillRune { get; set; }
    }
}
