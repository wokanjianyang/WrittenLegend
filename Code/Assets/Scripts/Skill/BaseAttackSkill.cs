using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class BaseAttackSkill : ASkill
    {
        public BaseAttackSkill(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
            this.skillGraphic = new BaseSkillGraphic(player,skillPanel.SkillData.SkillConfig);
        }


        public override long CalcFormula(APlayer enemy, float ratio)
        {
            //计算公式  ((攻击 - 防御) * 百分比系数 + 固定数值) * 暴击?.暴击倍率 * (伤害加成-伤害减免) * (幸运)
            
            long attack = GetRoleAttack() - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.Def); //攻击 - 防御
            attack = attack * SkillPanel.Percent / 100 + SkillPanel.Damage;  // *百分比系数 + 固定数值

            //暴击率 = 攻击者暴击率+技能暴击倍率-被攻击者暴击抵抗率
            long CritRate = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate) + SkillPanel.CritRate - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist);
            if (RandomHelper.RandomCritRate((int)CritRate))
            {
                //暴击倍率（ 不低于0 ） = 50基础爆伤+技能爆伤 + 攻击者爆伤 - 被攻击者爆伤减免
                long CritDamage = Math.Max(0, 50 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage) + SkillPanel.CritDamage - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist));
                attack = attack * (CritDamage + 100) / 100;
            }

            //伤害加成（不低于0） = 100基础伤害+技能伤害加成 + 攻击者伤害加成 — 被攻击者伤害减免 
            long DamageIncrea = Math.Max(0, 100 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea) + SkillPanel.DamageIncrea - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist));
            attack = attack * DamageIncrea / 100;

            //幸运，每点造成10%最终伤害
            long lucky = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky);
            attack = attack * (lucky * 10 + 100) / 100;

            //强制最少1点伤害
            return Math.Max(1, attack);
        }

        public override List<AttackData> GetAllTargets()
        {
            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SkillPanel.Dis, SkillPanel.Area);
            int EnemyNum = 0;

            List<AttackData> attackDatas = new List<AttackData>();

            foreach (var cell in allAttackCells)
            {
                if (EnemyNum >= SkillPanel.EnemyMax)
                {
                    break;
                }

                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.GroupId != SelfPlayer.GroupId) //不会攻击同组成员
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = enemy.ID,
                        Ratio = 1
                    });
                    EnemyNum++;
                }
            }

            return attackDatas;
        }

    }
}
