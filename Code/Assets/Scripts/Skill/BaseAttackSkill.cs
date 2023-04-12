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
        }


        public override long CalcFormula(APlayer enemy, float ratio)
        {
            //���㹫ʽ  ((���� - ����) * �ٷֱ�ϵ�� + �̶���ֵ) * ����?.�������� * (�˺��ӳ�-�˺�����) * (����)

            long attack = GetRoleAttack() - SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.Def); //���� - ����
            attack = attack * (SkillPanel.Percent + 100) / 100 + SkillPanel.Damage;  // *�ٷֱ�ϵ�� + �̶���ֵ

            //������ = �����߱�����+���ܱ�������-�������߱����ֿ���
            long CritRate = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritRate) + SkillPanel.CritRate - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritRateResist);
            if (RandomHelper.RandomCritRate((int)CritRate))
            {
                //�������ʣ� ������0 �� = 50�������� + �����߱��� - �������߱��˼���
                long CritDamage = Math.Max(0, 50 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamage) - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.CritDamageResist));
                attack = attack * (CritDamage + 100) / 100;
            }

            //�˺��ӳɣ�������0�� = 100�����˺� + �������˺��ӳ� �� ���������˺����� 
            long DamageIncrea = Math.Max(0, 100 + SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.DamageIncrea) - enemy.AttributeBonus.GetTotalAttr(AttributeEnum.DamageResist));
            attack = attack * DamageIncrea / 100;

            //���ˣ�ÿ�����10%�����˺�
            long lucky = SelfPlayer.AttributeBonus.GetTotalAttr(AttributeEnum.Lucky);
            attack = attack * (lucky * 10 + 100) / 100;

            //ǿ������1���˺�
            return Math.Max(1, attack);
        }

    }
}
