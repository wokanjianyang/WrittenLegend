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
            Vector3Int scale = Vector3Int.zero;
            if (SkillPanel.Area == AttackGeometryType.Square)
            {
                scale = new Vector3Int(SkillPanel.Column, SkillPanel.Row, 0);
            }
            else if (SkillPanel.Area == AttackGeometryType.FrontRow)
            {
                scale = new Vector3Int(SkillPanel.Dis, 1, 0);
            }

            this.skillGraphic?.PlayAnimation(playCells, scale);

            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                if (enemy != null)
                {
                    var dr = DamageHelper.CalcDamage(SelfPlayer.AttributeBonus, enemy.AttributeBonus, SkillPanel);
                    dr.FromId = attackData.Tid;
                    enemy.OnHit(dr);

                    foreach (EffectData effect in SkillPanel.EffectIdList.Values)
                    {
                        DoEffect(enemy, this.SelfPlayer, dr.Damage, effect);
                    }
                }
            }
        }

        abstract public List<AttackData> GetAllTargets();
        abstract public List<Vector3Int> GetPlayCells();
    }
}
