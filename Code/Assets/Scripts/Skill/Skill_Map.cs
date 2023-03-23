using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Map : BaseAttackSkill
    {
        public int Duration { get; set; }

        public Skill_Map(APlayer player, SkillData skillData) : base(player, skillData)
        {

        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            //Debug.Log($"获取技能:{(this.SkillData.Name)}施法目标");

            //施法中心为自己
            APlayer target = SelfPlayer;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(target.Cell, SkillData.Dis, SkillData.Area);

            return attackDatas;
        }

        public void Run(APlayer enemy)
        {
            Duration++;

            if (Duration >= SkillData.SkillConfig.Duration) {  //持续时间到了,销毁
                
            }
        }

        public override void Do()
        {
            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SkillData.Dis, SkillData.Area);

            foreach (var cell in allAttackCells)
            {
                MapCell mapCell = GameProcessor.Inst.MapData.GetMapCell(cell);
                mapCell.AddSkill(this);
            }
        }
    }
}
