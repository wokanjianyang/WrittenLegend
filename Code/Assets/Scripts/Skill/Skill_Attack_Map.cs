using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Map : Skill_Attack
    {
        public Skill_Attack_Map(APlayer player, SkillPanel skill) : base(player, skill)
        {
            this.skillGraphic = new SkillGraphic_Persistent(player, skill.SkillData.SkillConfig);
        }
        public override void Do()
        {
            List<Vector3Int> allAttackCells = GetPlayCells();

            this.skillGraphic.PlayAnimation(allAttackCells, Vector3Int.zero);

            foreach (var cell in allAttackCells)
            {
                MapCell mapCell = GameProcessor.Inst.MapData.GetMapCell(cell);
                if (mapCell != null) //处于地图边缘的时候
                {
                    mapCell.AddSkill(this);
                }
            }
        }
        public void Run(APlayer enemy)
        {
            if (enemy.GroupId == this.SelfPlayer.GroupId)
            {  //不对队友造成伤害
                return;
            }
            long damage = this.SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.PhyAtt) - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.Def);
            damage = damage > 1 ? damage : 1;

            enemy.OnHit(SelfPlayer.ID, damage);
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            //Debug.Log($"获取技能:{(this.SkillData.Name)}施法目标");

            //施法中心为自己
            APlayer target = SelfPlayer.Enemy;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(target.Cell, SkillPanel.Dis, SkillPanel.Area);

            foreach (var cell in allAttackCells)
            {
                attackDatas.Add(new AttackData()
                {
                    Tid = 0,
                    Ratio = 0
                });
            }

            return attackDatas;
        }
        public override List<Vector3Int> GetPlayCells()
        {
            return GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Cell, SkillPanel);
        }
    }
}
