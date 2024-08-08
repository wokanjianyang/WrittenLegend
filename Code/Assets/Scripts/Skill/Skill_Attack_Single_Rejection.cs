using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Single_Rejection : Skill_Attack
    {
        public Skill_Attack_Single_Rejection(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Single_Sequence(player, skill);
            }
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();
            List<Vector3Int> enmeyCells = new List<Vector3Int>();

            if (SelfPlayer.Enemy != null)
            {
                enmeyCells.Add(SelfPlayer.Enemy.Cell);
                attackDatas.Add(new AttackData()
                {
                    Tid = SelfPlayer.Enemy.ID,
                    Cell = SelfPlayer.Enemy.Cell,
                    Ratio = 0
                });
            }

            long divineMax = SkillPanel.DivineLevel * SkillPanel.DivineAttrConfig.Param;
            //Debug.Log("divineMax:" + divineMax);


            //施法中心为自己
            //APlayer target = SelfPlayer;
            Vector3Int from = SelfPlayer.Cell;
            Vector3Int to = SelfPlayer.Enemy.Cell;

            for (int i = 0; i < divineMax; i++)
            {
                List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(from, to, SkillPanel);
                allAttackCells.RemoveAll(m => enmeyCells.Contains(m));

                if (allAttackCells.Count <= 0)
                {
                    break;
                }

                //排序，从进到远
                allAttackCells = allAttackCells.OrderBy(m => Mathf.Abs(m.x - to.x) + Mathf.Abs(m.y - to.y) + Mathf.Abs(m.z - to.z)).ToList();

                foreach (var cell in allAttackCells)
                {
                    if (attackDatas.Count > divineMax)
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

                        from = cell;
                        enmeyCells.Add(cell);
                    }
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
