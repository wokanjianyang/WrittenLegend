using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Area : Skill_Attack
    {
        public Skill_Attack_Area(APlayer player, SkillPanel skillPanel, bool isShow) : base(player, skillPanel)
        {
            if (isShow)
            {
                if (skillPanel.Area == AttackGeometryType.FrontRow)
                {
                    SkillModelConfig SkillModelConfig = SkillModelConfigCategory.Instance.GetAll().Select(m => m.Value).Where(m => m.ModelName == this.SkillPanel.SkillData.SkillConfig.ModelName).FirstOrDefault();

                    if (SkillModelConfig.ScaleType == 1)
                    {
                        this.skillGraphic = new SkillGraphic_FrontRow(player, skillPanel);
                    }
                    else {
                        this.skillGraphic = new SkillGraphic_FrontRow1(player, skillPanel);
                    }
                }
                else if (skillPanel.Area == AttackGeometryType.Arc)
                {
                    this.skillGraphic = new SkillGraphic_Arc(player, skillPanel);
                }
                else
                {
                    this.skillGraphic = new SkillGraphic_Square(player, skillPanel);
                }
            }
        }

        public override List<AttackData> GetAllTargets()
        {
            List<Vector3Int> allAttackCells = GetPlayCells();

            List<AttackData> attackDatas = new List<AttackData>();

            foreach (var cell in allAttackCells)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.GroupId != SelfPlayer.GroupId) //不会攻击同组成员
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = enemy.ID,
                        Cell = cell,
                        Ratio = 1
                    });
                }
            }

            return attackDatas;
        }
        public override List<Vector3Int> GetPlayCells()
        {
            return GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Enemy.Cell, SkillPanel);
        }
    }
}
