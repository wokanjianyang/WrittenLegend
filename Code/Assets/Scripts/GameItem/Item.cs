using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Des { get; set; }

        /// <summary>
        ///  ��������
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// װ������ȼ�
        /// </summary>
        public int Level { get; set; }

        public int Gold { get; set; }

        /// <summary>
        /// �����б�
        /// </summary>
        public IDictionary<int, long> AttrList { get; set; }

        public IDictionary<int, SkillRune> skillList { get; set; }

        public int BoxId { get; set; } = -1;
    }
}
