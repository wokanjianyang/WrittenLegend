using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Area_From : Skill_Attack_Area
    {
        private SkillPanel FromSkill;

        public Skill_Attack_Area_From(APlayer player, SkillPanel skillPanel, SkillPanel from, bool isShow) : base(player, skillPanel, isShow)
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
                    else
                    {
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

    }
}
