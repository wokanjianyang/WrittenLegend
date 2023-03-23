using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class MapCell
    {
        private MapCell()
        {

        }
        public MapCell(Vector3Int cell)
        {
            this.cell = cell;
            this.skills = new List<Skill_Map>();
        }

        public List<Skill_Map> skills { get; private set; }

        public Vector3Int cell { get; set; }

        public void AddSkill(Skill_Map skill)
        {
            this.skills.Add(skill);

            //TODO 增加特效
        }

        public void RemoveSkill(Skill_Map skill) {
            this.skills.Remove(skill);

            //TODO移除特效
        }
    }
}
