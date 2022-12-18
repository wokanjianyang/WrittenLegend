using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Item
    {
        public Item() { 

        }
        public long Id { get; set; }

        public int ConfigId { get; set; }
        public string Name { get; set; }

        public string Des { get; set; }

        /// <summary>
        ///  ��������
        /// </summary>
        public ItemType Type { get; set; }

        /// <summary>
        /// װ������ȼ�
        /// </summary>
        public int Level { get; set; }

        public int Gold { get; set; }

        /// <summary>
        /// ����Ʒ�ʣ�������ʾ��������
        /// </summary>
        public int Quality { get; set; }

        /// <summary>
        /// �ѵ�����
        /// </summary>
        public int MaxNum { get; set; }

        /// <summary>
        /// �����б�
        /// </summary>
        public IDictionary<int, long> AttrList { get; set; }

        public IDictionary<int, SkillRune> skillList { get; set; }

        public int BoxId { get; set; } = -1;
    }

    public enum ItemType { 
        Normal = 0,
        Gold = 1,
        Equip = 2,
        SkillBox =3
    }
}
