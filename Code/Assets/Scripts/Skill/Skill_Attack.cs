using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    abstract public class Skill_Attack : ASkill
    {
        public Skill_Attack(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
        }

        public override bool IsCanUse()
        {
            //判断距离
            if (SelfPlayer.Enemy == null)
            {
                return false;
            }

            Vector3Int sp = SelfPlayer.Cell;
            Vector3Int ep = SelfPlayer.Enemy.Cell;

            if (SkillPanel.Area == AttackGeometryType.FrontRow || SkillPanel.Area == AttackGeometryType.Cross)
            {
                if (sp.x != ep.x && sp.y != ep.y) //判断是否在直线
                {
                    return false;
                }
            }

            int distance = Math.Abs(sp.x - ep.x) + Math.Abs(sp.y - ep.y) + Math.Abs(sp.z - ep.z);
            if (this.SkillPanel.Dis >= distance) //判断距离
            {
                return true;
            }

            return false;

        }

        public override void Do()
        {
            List<Vector3Int> playCells = GetPlayCells();
            this.skillGraphic?.PlayAnimation(playCells);

            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                if (enemy != null)
                {
                    //先行特效
                    foreach (EffectData effect in SkillPanel.EffectIdList.Values)
                    {
                        if (effect.Config.Priority < 0)
                        {
                            DoEffect(enemy, this.SelfPlayer, 0, effect);
                        }
                    }

                    var dr = DamageHelper.CalcDamage(SelfPlayer.AttributeBonus, enemy.AttributeBonus, SkillPanel);
                    dr.FromId = attackData.Tid;
                    enemy.OnHit(dr);

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
                            long total = dr.Damage * effect.Percent / 100;
                            //Debug.Log("restor:" + total);
                            DoEffect(enemy, this.SelfPlayer, total, effect);
                        }
                    }
                }
            }
        }

        abstract public List<AttackData> GetAllTargets();
        abstract public List<Vector3Int> GetPlayCells();
    }
}
