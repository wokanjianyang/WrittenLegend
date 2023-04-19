using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Map : BaseAttackSkill
    {
        public Skill_Map(APlayer player, SkillPanel skill) : base(player, skill)
        {
            this.skillGraphic = new AttackSkillGraphic(player, skill.SkillData.SkillConfig.Name);
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            //Debug.Log($"获取技能:{(this.SkillData.Name)}施法目标");

            //施法中心为自己
            APlayer target = SelfPlayer;

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

        public override void Do()
        {
            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SkillPanel.Dis, SkillPanel.Area);

            foreach (var cell in allAttackCells)
            {
                MapCell mapCell = GameProcessor.Inst.MapData.GetMapCell(cell);
                if (mapCell != null) //处于地图边缘的时候
                {
                    mapCell.AddSkill(this);
                }
                this.skillGraphic.PlayAnimation(cell);
            }

        }
    }
}
