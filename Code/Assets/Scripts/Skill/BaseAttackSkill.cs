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
            //���㹫ʽ  ((���� - ����) * �ٷֱ�ϵ�� + �̶���ֵ) * ����?.�������� * (�˺��ӳ�-�˺�����) * (����)

            long attack = GetRoleAttack() - SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.Def); //���� - ����
            attack = attack * SkillPanel.Percent / 100 + SkillPanel.Damage;  // *�ٷֱ�ϵ�� + �̶���ֵ

            //������ = �����߱�����+���ܱ�������-�������߱����ֿ���
            long CritRate = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate) + SkillPanel.CritRate - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist);
            if (RandomHelper.RandomCritRate((int)CritRate))
            {
                //�������ʣ� ������0 �� = 50��������+���ܱ��� + �����߱��� - �������߱��˼���
                long CritDamage = Math.Max(0, 50 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage) + SkillPanel.CritDamage - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist));
                attack = attack * (CritDamage + 100) / 100;
            }

            //�˺��ӳɣ�������0�� = 100�����˺�+�����˺��ӳ� + �������˺��ӳ� �� ���������˺����� 
            long DamageIncrea = Math.Max(0, 100 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea) + SkillPanel.DamageIncrea - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist));
            attack = attack * DamageIncrea / 100;

            //���ˣ�ÿ�����10%�����˺�
            long lucky = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky);
            attack = attack * (lucky * 10 + 100) / 100;

            //ǿ������1���˺�
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
                if (enemy != null && enemy.GroupId != SelfPlayer.GroupId) //���ṥ��ͬ���Ա
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
