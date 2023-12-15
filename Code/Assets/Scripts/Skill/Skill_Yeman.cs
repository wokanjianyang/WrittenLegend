using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Yeman : Skill_Attack
    {
        public Skill_Yeman(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Normal(player, skill);
            }
        }

        public override bool IsCanUse()
        {
            return base.IsCanUse() && GetMoveCell() != Vector3Int.zero;
        }

        public override void Do()
        {
            var moveCell = GetMoveCell();
            this.SelfPlayer.Move(moveCell);

            base.Do();
        }

        private Vector3Int GetMoveCell()
        {
            if (SelfPlayer.Enemy == null)
            {
                return Vector3Int.zero;
            }

            Vector3Int dis = SelfPlayer.Cell - SelfPlayer.Enemy.Cell;
            if (Math.Abs(dis.x + dis.y) == 1)
            {
                return SelfPlayer.Cell;
            }


            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Enemy.Cell, SkillPanel);

            Vector3Int selfCell = SelfPlayer.Cell;
            allAttackCells = allAttackCells.OrderBy(m => Math.Abs(m.x - selfCell.x) + Math.Abs(m.y - selfCell.y) + Math.Abs(m.z - selfCell.z)).ToList();

            foreach (var cell in allAttackCells)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy == null)
                {
                    return cell;
                }
            }

            return Vector3Int.zero;
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
