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
        public List<SkillMapState> skills { get; private set; }
        public Vector3Int cell { get; set; }

        public MapCell(Vector3Int cell)
        {
            this.cell = cell;
            this.skills = new List<SkillMapState>();
        }

        public void AddSkill(Skill_Attack_Map skill)
        {
            this.skills.Add(new SkillMapState(skill));
            //TODO 增加特效
        }

        public void DoEvent()
        {
            APlayer player = GameProcessor.Inst.PlayerManager.GetPlayer(cell);

            //触发技能
            foreach (SkillMapState skill in skills)
            {
                skill.Run(player);
            }

            //持续时间到了，移除技能
            List<SkillMapState> overList = skills.FindAll(m => m.IsOver());
            foreach (SkillMapState skill in overList)
            {
                //TODO移除特效  
                this.skills.Remove(skill);
            }
        }

        public void Clear()
        {
            this.skills = new List<SkillMapState>();
        }
    }
}
