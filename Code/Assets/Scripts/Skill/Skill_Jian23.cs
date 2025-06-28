using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Jian23 : ASkill
    {
        private SkillPanel FromSkill;

        public Skill_Jian23(APlayer player, SkillPanel skill, SkillPanel fromSkill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Chediding(player, skill);
            }

            this.FromSkill = fromSkill;

            this.FromSkill.IgnoreDef += this.SkillPanel.IgnoreDef;
        }

        public override bool IsCanUse()
        {
            return false;
        }

        public override void Do(SkillRunType runType)
        {

        }

        public override void Do(DamageResult baseDr)
        {
            List<Vector3Int> playCells = GetPlayCells();

            this.skillGraphic?.PlayAnimation(playCells);

            List<AttackData> attackDataCache = GetAllTargets();
            foreach (var attackData in attackDataCache)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                if (enemy != null)
                {
                    if (DamageHelper.IsMiss(SelfPlayer, enemy))
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

                    double dm = baseDr.Damage * 0.2 * SkillPanel.Percent;
                    double edm = baseDr.ExtendDamage * 0.2 * SkillPanel.Percent;

                    //Debug.Log("dm:" + StringHelper.FormatNumber(dm) + "  edm:" + StringHelper.FormatNumber(edm));
                    var dr = DamageHelper.CalcDamage(SelfPlayer.AttributeBonus, enemy.AttributeBonus, FromSkill);

                    dr.Damage = dr.Damage * SkillPanel.Percent / 100;

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

        public List<AttackData> GetAllTargets()
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

        public List<Vector3Int> GetPlayCells()
        {
            return GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Enemy.Cell, SelfPlayer.Cell, SkillPanel);
        }
    }
}
