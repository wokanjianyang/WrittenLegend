using System.Collections.Generic;

namespace Game
{
    public class Skill_Sweep : BaseAttackSkill
    {
        public override List<AttackData> GetAllTargets(int tid)
        {
            var mainEnemy = GameProcessor.Inst.PlayerManager.GetPlayer(tid);
            var dir = mainEnemy.Cell - this.SelfPlayer.Cell;
            var allAttackCells = GameProcessor.Inst.MapProcessor.GetAttackRangeCell(this.SelfPlayer.Cell, dir, 3, AttackGeometryType.FrontRow);
            float ratio = 1;
            List<AttackData> attackDatas = new List<AttackData>();
            foreach (var cell in allAttackCells)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null)
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = enemy.ID,
                        Ratio = ratio
                    });
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
            return this.SelfPlayer.Logic.GetAttributeFloat(AttributeEnum.PhyAtt) * ratio * 10;
        }
    }
}
