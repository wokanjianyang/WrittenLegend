using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Sweep : BaseAttackSkill
    {
        public Skill_Sweep(APlayer player, SkillPanel skill) : base(player, skill)
        {
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            //Debug.Log($"获取技能:{(this.SkillData.Name)}施法目标");

            //施法中心为自己
            APlayer target = SelfPlayer;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(target.Cell, SkillPanel.Dis, SkillPanel.Area);
            float ratio = allAttackCells.Count;

            //排序，从进到远
            Vector3Int mainCell = SelfPlayer.Cell;
            Vector3Int selfCell = SelfPlayer.Cell;
            allAttackCells = allAttackCells.OrderBy(m => Mathf.Abs(m.x - mainCell.x) + Mathf.Abs(m.y - mainCell.y) + Mathf.Abs(m.z - mainCell.z)
            + Mathf.Abs(m.x - selfCell.x) + Mathf.Abs(m.y - selfCell.y) + Mathf.Abs(m.z - selfCell.z)).ToList();

            int EnemyNum = 0;


            foreach (var cell in allAttackCells)
            {
                if (EnemyNum >= SkillPanel.EnemyMax)
                {
                    break;
                }

                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.GroupId != SelfPlayer.GroupId) //不会攻击同组成员
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = enemy.ID,
                        Ratio = ratio
                    });
                    EnemyNum++;
                }

                if (ratio > 0.1f)
                {
                    ratio -= 0.1f;
                }
            }

            return attackDatas;
        }
    }
}
