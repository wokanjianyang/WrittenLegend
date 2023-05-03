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
            //Debug.Log($"ʹ�ü���:{(this.SkillPanel.SkillData.SkillConfig.Name)},ʩ��Ŀ��Ϊ�Լ�");

            List<AttackData> attackDatas = new List<AttackData>();

            List<Vector3Int> allAttackCells = GameProcessor.Inst.MapData.GetAttackRangeCell(SelfPlayer.Cell, SkillPanel.Dis, SkillPanel.Area);

            List<APlayer> teamList = new List<APlayer>();

            teamList.Add(SelfPlayer);

            foreach (var cell in allAttackCells)
            {
                var enemy = GameProcessor.Inst.PlayerManager.GetPlayer(cell);
                if (enemy != null && enemy.GroupId == SelfPlayer.GroupId && enemy.ID != SelfPlayer.ID) //ֻ�ظ�ͬ���Ա,�Լ��Ѿ��ӽ�ȥ��
                {
                    teamList.Add(enemy);
                }
            }

            //����ʧѪ������
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
                SkillConfig skillConfig = SkillPanel.SkillData.SkillConfig;
                if (skillConfig.EffectIdList != null && skillConfig.EffectIdList.Length > 0)
                {
                    foreach (int EffectId in skillConfig.EffectIdList)
                    {
                        EffectConfig config = EffectConfigCategory.Instance.Get(EffectId);

                        var effectTarget = config.TargetType == 1 ? this.SelfPlayer : teamer; //1 Ϊ�����Լ� 2 Ϊ���õ���

                        if (config.Duration > 0)
                        {  //����Buff
                            effectTarget.AddEffect(EffectId, this.SelfPlayer);
                        }
                        else
                        {
                            effectTarget.RunEffect(EffectId, this.SelfPlayer);
                        }
                    }
                }

                this.skillGraphic?.PlayAnimation(teamer.Cell);
            }
        }

        public long CalcFormula()
        {
            //�ָ����Ʊ����������˵�
            long attack = GetRoleAttack();  
            attack = attack * SkillPanel.Percent / 100 + SkillPanel.Damage;
            return attack;
        }
    }
}
