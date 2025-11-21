using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Skill_Jufengpo : Skill_Attack_Area
    {
        private SkillPanel FromSkill;

        public Skill_Jufengpo(APlayer player, SkillPanel skillPanel, SkillPanel from, bool isShow) : base(player, skillPanel, isShow)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Jufengpo(player, skillPanel);
            }

            this.FromSkill = from;

            if (this.FromSkill != null)
            {
                this.FromSkill.IgnoreDef += this.SkillPanel.IgnoreDef;
            }
        }

        public override void Do(SkillRunType runType)
        {
            List<Vector3Int> playCells = GetPlayCells();

            this.skillGraphic?.PlayAnimation(playCells);

            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                if (enemy != null)
                {
                    if (DamageHelper.IsMiss(SelfPlayer, enemy, SkillPanel.Accuracy))
                    {
                        enemy.ShowMiss();
                        return;
                    }

                    //先行特效
                    foreach (EffectData effect in SkillPanel.EffectIdList.Values)
                    {
                        if (effect.Config.Priority < 0)
                        {
                            DoEffect(enemy, this.SelfPlayer, 0, 0, effect);
                        }
                    }

                    //Debug.Log("dm:" + StringHelper.FormatNumber(dm) + "  edm:" + StringHelper.FormatNumber(edm));
                    var dr = DamageHelper.CalcDamage(SelfPlayer.AttributeBonus, enemy.AttributeBonus, FromSkill != null ? FromSkill : SkillPanel);

                    //Debug.Log("base .damage：" + dr.Damage);

                    if (FromSkill != null && FromSkill.DivineLevel > 0)
                    {
                        double disPercent = this.FromSkill.DivineLevel * 30; //30为了和刺杀剑术的神技一样生效

                        //Debug.Log("DivineLevel.Percent：" + disPercent);

                        dr.Damage = dr.Damage * (100 + disPercent) / 100;
                    }

                    dr.Damage = dr.Damage * (100 + SkillPanel.Percent) / 100;

                    //Debug.Log("SkillPanel.Percent：" + SkillPanel.Percent);

                    dr.FromId = attackData.Tid;
                    enemy.OnHit(dr);

                    //后行特效
                    foreach (EffectData effect in SkillPanel.EffectIdList.Values)
                    {
                        if (effect.Config.Priority >= 0)
                        {
                            double total = dr.Damage * effect.Percent / 100;
                            //Debug.Log("restor:" + total);
                            DoEffect(enemy, this.SelfPlayer, total, 0, effect);
                        }
                    }
                }
            }
        }


        public override List<AttackData> GetAllTargets()
        {
            List<AttackData> attackDatas = new List<AttackData>();

            if (SelfPlayer.Enemy != null)
            {
                attackDatas.Add(new AttackData()
                {
                    Tid = SelfPlayer.Enemy.ID,
                    Cell = SelfPlayer.Enemy.Cell,
                    Ratio = 0
                });
            }

            if (attackDatas.Count >= SkillPanel.EnemyMax)  //如果只能攻击一个，则优先攻击目标
            {
                return attackDatas;
            }

            //Debug.Log($"获取技能:{(this.SkillPanel.SkillData.SkillConfig.Name)}施法目标");

            //施法中心为自己
            APlayer target = SelfPlayer;

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Enemy.Cell, SkillPanel);
            allAttackCells.Remove(SelfPlayer.Enemy.Cell);

            //排序，从进到远
            Vector3Int selfCell = SelfPlayer.Cell;
            allAttackCells = allAttackCells.OrderBy(m => Mathf.Abs(m.x - selfCell.x) + Mathf.Abs(m.y - selfCell.y) + Mathf.Abs(m.z - selfCell.z)).ToList();


            foreach (var cell in allAttackCells)
            {
                if (attackDatas.Count >= SkillPanel.EnemyMax)
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
