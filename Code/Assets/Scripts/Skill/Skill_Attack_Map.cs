using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Map : Skill_Attack
    {
        public Skill_Attack_Map(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                SkillModelConfig SkillModelConfig = SkillModelConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ModelName == this.SkillPanel.SkillData.SkillConfig.ModelName).FirstOrDefault();

                if (SkillModelConfig.ScaleType == 1)
                {
                    this.skillGraphic = new SkillGraphic_Persistent(player, skill);
                }
                else
                {
                    this.skillGraphic = new SkillGraphic_Persistent_Square(player, skill);
                }
            }
        }
        public override void Do()
        {
            List<Vector3Int> allAttackCells = GetPlayCells();
            this.skillGraphic?.PlayAnimation(allAttackCells);

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

            if (DamageHelper.IsMiss(SelfPlayer, enemy))
            {
                enemy.ShowMiss();
                return;
            }

            var dr = DamageHelper.CalcDamage(this.SelfPlayer.AttributeBonus, enemy.AttributeBonus, this.SkillPanel);
            dr.FromId = this.SelfPlayer.ID;
            enemy.OnHit(dr);
        }

        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();
            return attackDatas;
        }

        public override List<Vector3Int> GetPlayCells()
        {
            return GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Enemy.Cell, SkillPanel);
        }
    }
}
