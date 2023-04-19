using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    abstract public class SkillGraphic
    {
        public APlayer SelfPlayer { get; private set; }

        public string SkillName { get; private set; }

        public SkillGraphic(APlayer player,string skillName)
        {
            this.SelfPlayer = player;
            this.SkillName = skillName;
        }

        abstract public void PlayAnimation(Vector3Int cell);
    }
}
