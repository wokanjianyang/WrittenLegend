using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    abstract public class SkillGraphic
    {
        public APlayer SelfPlayer { get; private set; }

        public SkillConfig SkillConfig { get; private set; }

        public SkillGraphic(APlayer player, SkillConfig skillConfig)
        {
            this.SelfPlayer = player;
            this.SkillConfig = skillConfig;
        }

        abstract public void PlayAnimation(List<Vector3Int> cells, Vector3Int scale);

        public void PlayAnimation(List<Vector3Int> cells)
        {
            this.PlayAnimation(cells, Vector3Int.zero);
        }

        public void PlayAnimation(Vector3Int cell)
        {
            List<Vector3Int> cells = new List<Vector3Int>();
            cells.Add(cell);
            this.PlayAnimation(cells, Vector3Int.zero);
        }
    }
}
