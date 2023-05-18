using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class Skill_Restore : BaseAttackSkill
    {
        public Skill_Restore(APlayer player, SkillPanel skillPanel) : base(player, skillPanel)
        {
            this.skillGraphic = new SweepSkillGraphic(player, skillPanel.SkillData.SkillConfig);
        }

        public override List<AttackData> GetAllTargets()
        {
            //Debug.Log($"使用技能:{(this.SkillPanel.SkillData.SkillConfig.Name)},施法目标为自己");

            List<AttackData> attackDatas = new List<AttackData>();

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SkillPanel.Dis, SkillPanel.Area);

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
            teamList = teamList.OrderBy(m => m.AttributeBonus.GetTotalAttr(AttributeEnum.HP) - m.HP).ToList();

            foreach (var teamer in teamList)
            {
                if (teamer.AttributeBonus.GetTotalAttr(AttributeEnum.HP) > teamer.HP)
                {
                    attackDatas.Add(new AttackData()
                    {
                        Tid = teamer.ID,
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

        public override bool IsCanUse()
        {
            return GetAllTargets().Count > 0;
        }

        public override void Do()
        {
            List<AttackData> attackDataCache = GetAllTargets();

            foreach (var attackData in attackDataCache)
            {
                var teamer = GameProcessor.Inst.PlayerManager.GetPlayer(attackData.Tid);

                var hp = CalcFormula();
                teamer.OnRestore(attackData.Tid, hp);

                //Buff
                foreach (int effectId in SkillPanel.EffectIdList.Keys)
                {
                    DoEffect(this.SelfPlayer, this.SelfPlayer, hp, SkillPanel.EffectIdList[effectId]);
                }

                this.skillGraphic?.PlayAnimation(teamer.Cell);
            }
        }

        public long CalcFormula()
        {
            //恢复不计暴击增伤幸运等
            int role = SkillPanel.SkillData.SkillConfig.Role;

            long roleAttr = SelfPlayer.GetRoleAttack(role) * (100 + SkillPanel.AttrIncrea) / 100;  //职业攻击

            //技能系数
            long attack = roleAttr * (SkillPanel.Percent + SelfPlayer.GetRolePercent(role)) / 100 + SkillPanel.Damage + SelfPlayer.GetRoleDamage(role);  // *百分比系数 + 固定数值

            return attack;
        }
    }
}
