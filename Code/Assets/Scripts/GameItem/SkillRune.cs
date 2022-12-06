using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SkillRune
    {
        public int SkillId { get; set; }
        public string Name { get; set; }

        public string Des { get; set; }

        /// <summary>
        ///  ¿‡–Õ
        /// </summary>
        public int Max { get; set; }

        public int Incre { get; set; }

        public int SuitId { get; set; }
    }

    public enum SkillRuneType { 
        Damage = 0,
        Percent ,
        Dis,
        MaxNum,
        Effect
    }
}
