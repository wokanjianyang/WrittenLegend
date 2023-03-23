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

            //Debug.Log($"��ȡ����:{(this.SkillData.Name)}ʩ��Ŀ��");

            //ʩ������Ϊ�Լ�
            APlayer target = SelfPlayer;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(target.Cell, SkillData.Dis, SkillData.Area);

            return attackDatas;
        }

        public void Run(APlayer enemy)
        {
            Duration++;

            if (Duration >= SkillData.SkillConfig.Duration) {  //����ʱ�䵽��,����
                
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
