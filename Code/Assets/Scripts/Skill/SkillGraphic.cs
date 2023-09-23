using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    abstract public class SkillGraphic
    {
        public APlayer SelfPlayer { get; private set; }

        public SkillPanel SkillPanel { get; private set; }

        public SkillGraphic(APlayer player, SkillPanel skill)
        {
            this.SelfPlayer = player;
            this.SkillPanel = skill;
        }

        virtual public void PlayAnimation(List<Vector3Int> cells)
        {

        }

        public void PlayAnimation(Vector3Int cell)
        {
            List<Vector3Int> cells = new List<Vector3Int>();
            cells.Add(cell);
            this.PlayAnimation(cells);
        }
    }
}
