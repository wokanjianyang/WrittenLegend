using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Sweep : BaseAttackSkill
    {
        public Skill_Sweep(APlayer player, SkillData skillData) : base(player, skillData)
        {
        }

        public override List<AttackData> GetAllTargets(int tid)
        {
            List<AttackData> attackDatas = new List<AttackData>();

            if (tid <= 0)
                return attackDatas;

            Debug.Log($"使用技能:{(this.SkillData.Name)}");

            APlayer mainEnemy = GameProcessor.Inst.PlayerManager.GetPlayer(tid);
            var dir = mainEnemy.Cell - this.SelfPlayer.Cell;

            APlayer target = SkillData.Center == SkillCenter.Self ? SelfPlayer : mainEnemy;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(target.Cell, dir, SkillData.Dis, SkillData.Area);
            float ratio = allAttackCells.Count;

            //排序，从进到远
            Vector3Int mainCell = mainEnemy.Cell;
            Vector3Int selfCell = SelfPlayer.Cell;
            allAttackCells = allAttackCells.OrderBy(m => Mathf.Abs(m.x - mainCell.x) + Mathf.Abs(m.y - mainCell.y) + Mathf.Abs(m.z - mainCell.z)
            + Mathf.Abs(m.x - selfCell.x) + Mathf.Abs(m.y - selfCell.y) + Mathf.Abs(m.z - selfCell.z)).ToList();

            int EnemyNum = 0;


            foreach (var cell in allAttackCells)
            {
                if (EnemyNum >= SkillData.EnemyMax)
                {
                    break;
                }

                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.ID != SelfPlayer.ID && enemy.MasterId != SelfPlayer.ID)
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
        
        public override float CalcFormula(APlayer player, float ratio)
        {
            return this.SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt) * ratio;
        }
    }
}
