using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Restore : ASkill
    {
        public Skill_Restore(APlayer player, SkillPanel skill, bool isShow) : base(player, skill)
        {
            if (isShow)
            {
                this.skillGraphic = new SkillGraphic_Single(player, skill);
            }
        }

        public override bool IsCanUse()
        {
            return GetAllTargets().Count > 0;
        }

        public override void Do()
        {
            List<AttackData> attackDataCache = GetAllTargets();

            List<Vector3Int> cells = attackDataCache.Select(m => m.Cell).ToList();
            this.skillGraphic?.PlayAnimation(cells);

            foreach (var attackData in attackDataCache)
            {
                var teamer = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                var hp = CalcFormula();
                teamer.OnRestore(attackData.Tid, hp);

                //Debug.Log("Restore Base :" + hp);

                //Buff
                foreach (EffectData effect in SkillPanel.EffectIdList.Values)
                {
                    //Debug.Log("Restore Effect Percent:" + effect.Percent);
                    double total = hp * effect.Percent / 100;
                    //Debug.Log("Restore Effect :" + total);

                    DoEffect(this.SelfPlayer, this.SelfPlayer, total, 0, effect);
                }
            }
        }

        public double CalcFormula()
        {
            //恢复不计暴击增伤幸运等
            int role = SkillPanel.SkillData.SkillConfig.Role;

            double roleAttr = SelfPlayer.GetRoleAttack(role, true) * (100 + SkillPanel.AttrIncrea) / 100;  //职业攻击

            //技能系数
            double attack = roleAttr * (SkillPanel.Percent + SelfPlayer.GetRolePercent(role)) / 100 + SkillPanel.Damage + SelfPlayer.GetRoleDamage(role);  // *百分比系数 + 固定数值

            return attack;
        }

        public List<AttackData> GetAllTargets()
        {
            //Debug.Log($"使用技能:{(this.SkillPanel.SkillData.SkillConfig.Name)},施法目标为自己");

            List<AttackData> attackDatas = new List<AttackData>();

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SelfPlayer.Cell, SkillPanel);

            List<APlayer> teamList = new List<APlayer>();

            teamList.Add(SelfPlayer);

            foreach (var cell in allAttackCells)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.GroupId == SelfPlayer.GroupId && enemy.ID != SelfPlayer.ID) //只回复同组成员,自己已经加进去了
                {
                    teamList.Add(enemy);
                }
            }

            //按损失血量排序
            teamList = teamList.OrderBy(m => m.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP) - m.HP).ToList();

            foreach (var teamer in teamList)
            {
                if (teamer.AttributeBonus.GetAttackDoubleAttr(AttributeEnum.HP) > teamer.HP)
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = teamer.ID,
                        Cell = teamer.Cell,
                        Ratio = 1
                    });
                }

                if (attackDatas.Count >= SkillPanel.EnemyMax)
                {
                    break;
                }
            }

            return attackDatas;
        }
    }
}
