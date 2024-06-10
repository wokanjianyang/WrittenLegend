using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Distance_Rise : Skill_Attack_Area
    {

        public Skill_Attack_Distance_Rise(APlayer player, SkillPanel skillPanel, bool isShow) : base(player, skillPanel, isShow)
        {

        }

        public override void Do()
        {
            double baseHp = 0;

            List<Vector3Int> playCells = GetPlayCells();
            this.skillGraphic?.PlayAnimation(playCells);

            SkillState orbState = this.SelfPlayer.GetSkillByPriority(-100);

            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                if (enemy != null)
                {
                    if (DamageHelper.IsMiss(SelfPlayer, enemy))
                    {
                        enemy.ShowMiss();
                        continue;
                    }

                    //先行特效
                    foreach (EffectData effect in SkillPanel.EffectIdList.Values)
                    {
                        if (effect.Config.Priority < 0)
                        {
                            DoEffect(enemy, this.SelfPlayer, 0, 0, effect);
                        }
                    }

                    if (orbState != null)
                    {
                        foreach (EffectData effect in orbState.SkillPanel.EffectIdList.Values)
                        {
                            if (effect.Config.Priority < 0)
                            {
                                DoEffect(enemy, this.SelfPlayer, 0, 0, effect);
                                //Debug.Log("Run Ring Effect:" + effect.Config.Name);
                            }
                        }
                    }


                    int distance = this.CalDistance(SelfPlayer.Cell, enemy.Cell) - 1;
                    double percent = distance * this.SkillPanel.DivineLevel * this.SkillPanel.DivineAttrConfig.Param;
                    Debug.Log("distance:" + distance + " rise percent:" + percent);

                    var dr = DamageHelper.CalcDamage(SelfPlayer.AttributeBonus, enemy.AttributeBonus, SkillPanel);
                    dr.Damage = dr.Damage * (100 + percent) / 100;

                    dr.FromId = attackData.Tid;
                    enemy.OnHit(dr);

                    if (enemy.ID == SelfPlayer.Enemy.ID)
                    {
                        baseHp = dr.Damage;
                    }

                    //if (this.SelfPlayer.Camp == PlayerType.Valet)
                    //{
                    //    Debug.Log(SkillPanel.SkillData.SkillConfig.Name + ":" + dr.Damage);
                    //}

                    //if (this.SkillPanel.SkillId < 4000)
                    //{ Debug.Log(SkillPanel.SkillData.SkillConfig.Name + ":" + dr.Damage); }

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

                    //法球
                    if (orbState != null)
                    {
                        foreach (EffectData effect in orbState.SkillPanel.EffectIdList.Values)
                        {
                            if (effect.Config.Priority >= 0)
                            {
                                double total = dr.Damage * effect.Percent / 100;
                                //Debug.Log("restor:" + total);
                                DoEffect(enemy, this.SelfPlayer, total, 0, effect);
                                //Debug.Log("Run Ring Effect:" + effect.Config.Name);
                            }
                        }
                    }
                }
            }

            if (orbState != null)
            {
                orbState.Do();
            }

            if (SkillPanel.SkillData.SkillConfig.Role == (int)RoleType.Warrior)
            {
                //do Chediding
                SkillState skillChediding = SelfPlayer.SelectSkillList.Where(m => m.SkillPanel.SkillId == 1010).FirstOrDefault();
                if (skillChediding != null && baseHp > 0 && RandomHelper.RandomNumber(1, 6) <= 1)
                {
                    skillChediding.Do(baseHp);
                }
            }
        }

    }
}
