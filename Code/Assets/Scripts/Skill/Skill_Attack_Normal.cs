using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Normal : Skill_Attack
    {
        public Skill_Attack_Normal(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Normal(player, skill);
            }
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            if (SelfPlayer.Enemy != null) //不会攻击同组成员
            {
                attackDatas.Add(new AttackData()
                {
                    Tid = SelfPlayer.Enemy.ID,
                    Cell = SelfPlayer.Enemy.Cell,
                    Ratio = 0
                });
            }

            return attackDatas;
        }

        public override List<Vector3Int> GetPlayCells()
        {
            return GetAllTargets().Select(m => m.Cell).ToList();
        }
    }
}
