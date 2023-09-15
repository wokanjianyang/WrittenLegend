using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Single : Skill_Attack
    {
        public Skill_Attack_Single(APlayer player, SkillPanel skill,bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Single(player, skill);
            }
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            if (SelfPlayer.Enemy != null)
            {
                attackDatas.Add(new AttackData()
                {
                    Tid = SelfPlayer.Enemy.ID,
                    Cell = SelfPlayer.Enemy.Cell,
                    Ratio = 0
                });
            }

            if (attackDatas.Count >= SkillPanel.EnemyMax)  //如果只能攻击一个，则优先攻击目标
            {
                return attackDatas;
            }

            //Debug.Log($"获取技能:{(this.SkillPanel.SkillData.SkillConfig.Name)}施法目标");

            //施法中心为自己
            APlayer target = SelfPlayer;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Enemy.Cell, SkillPanel);
            allAttackCells.Remove(SelfPlayer.Enemy.Cell);

            //排序，从进到远
            Vector3Int selfCell = SelfPlayer.Cell;
            allAttackCells = allAttackCells.OrderBy(m => Mathf.Abs(m.x - selfCell.x) + Mathf.Abs(m.y - selfCell.y) + Mathf.Abs(m.z - selfCell.z)).ToList();


            foreach (var cell in allAttackCells)
            {
                if (attackDatas.Count >= SkillPanel.EnemyMax)
                {
                    break;
                }

                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.IsSurvice && enemy.GroupId != SelfPlayer.GroupId) //不会攻击同组成员
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = enemy.ID,
                        Cell = cell,
                        Ratio = 0
                    });
                }
            }

            return attackDatas;
        }

        public override List<Vector3Int> GetPlayCells()
        {
            return GetAllTargets().Select(m => m.Cell).ToList();
        }
    }
}
