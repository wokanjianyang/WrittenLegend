using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillData
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public string Des { get; set; }

        public int CD { get; set; }

        /// <summary>
        /// ʩ������ 1 ���� 2����
        /// </summary>
        public int CastType { get; set; }

        /// <summary>
        /// ְҵ 1 սʿ 2 ��ʦ 3 ��ʿ
        /// </summary>
        public int Role { get; set; }

        public int Level { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public int Dis { get; set; }

        /// <summary>
        /// �������� self enemy
        /// </summary>
        public SkillCenter Center { get; set; }

        /// <summary>
        /// ������������
        /// </summary>
        public AttackGeometryType Area { get; set; } 

        /// <summary>
        /// ��󹥻���������
        /// </summary>
        public int EnemyMax { get; set; }

        /// <summary>
        /// �˺�����
        /// </summary>
        public int Percent { get; set; }

        /// <summary>
        /// �̶��˺�
        /// </summary>
        public int Damage { get; set; }
    }
}
