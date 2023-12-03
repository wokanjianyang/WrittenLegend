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
            //如果还有附加特效
            var moveCell = GetMoveCell();
            this.SelfPlayer.SetPosition(moveCell, false);

            base.Do();
        }

        private Vector3Int GetMoveCell()
        {
            if (SelfPlayer.Enemy == null)
            {
                return Vector3Int.zero;
            }

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Enemy.Cell, SkillPanel);

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
