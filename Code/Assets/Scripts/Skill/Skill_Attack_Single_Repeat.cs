using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Attack_Single_Repeat : Skill_Attack
    {
        public Skill_Attack_Single_Repeat(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Single(player, skill);
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

        public override void Do()
        {

            double baseHp = 0;

            SkillState orbState = this.SelfPlayer.GetSkillByPriority(-100);


            List<AttackData> attackDataCache = GetAllTargets();

            if (attackDataCache.Count <= 0)
            {
                return;
            }

            int ac = 0;
            long repeatMax = SkillPanel.DivineLevel * SkillPanel.DivineAttrConfig.Param;

            for (int i = 0; i < repeatMax; i++)
            {
                for (int j = 0; j < attackDataCache.Count; j++)
                {
                    var attackData = attackDataCache[j];
                    var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                    if (enemy != null && enemy.IsSurvice)
                    {
                        this.skillGraphic?.PlayAnimation(enemy.Cell);

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

                        var dr = DamageHelper.CalcDamage(SelfPlayer.AttributeBonus, enemy.AttributeBonus, SkillPanel);
                        dr.FromId = attackData.Tid;
                        enemy.OnHit(dr);

                        if (enemy.ID == SelfPlayer.Enemy.ID)
                        {
                            baseHp = dr.Damage;
                        }

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

                    ac++;

                    if (ac >= SkillPanel.EnemyMax)
                    {
                        break;
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
