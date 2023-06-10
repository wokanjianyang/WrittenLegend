using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class PlayerHelper
    {
        public const int MAX_EQUIP_COUNT = 8;

        public const int Max_Level = 350;

        public static Dictionary<string, string> PlayerAttributeMap = new Dictionary<string, string>()
        {
            {nameof(AttributeEnum.Color),"��ɫ" },
            {nameof(AttributeEnum.Name),"����" },
            {nameof(AttributeEnum.Level),"�ȼ�" },
            {nameof(AttributeEnum.Exp), "����ֵ" },
            {nameof(AttributeEnum.Power), "ս��" },
            {nameof(AttributeEnum.HP), "����ֵ" },
            {nameof(AttributeEnum.PhyAtt), "������" },
            {nameof(AttributeEnum.MagicAtt),"ħ������" },
            {nameof(AttributeEnum.SpiritAtt), "��������" },
            {nameof(AttributeEnum.Def), "����" },
            {nameof(AttributeEnum.Speed), "����" },
            {nameof(AttributeEnum.Lucky), "����" },
            {nameof(AttributeEnum.CritRate), "������" },
            {nameof(AttributeEnum.CritDamage), "��������" },
            {nameof(AttributeEnum.CritRateResist), "����" },
            {nameof(AttributeEnum.CritDamageResist), "���˼���" },
            {nameof(AttributeEnum.DamageIncrea), "�˺�����" },
            {nameof(AttributeEnum.DamageResist), "�˺�����" },
            {nameof(AttributeEnum.AttIncrea), "�����ӳ�" },
            {nameof(AttributeEnum.HpIncrea), "�����ӳ�" },
            {nameof(AttributeEnum.InheritIncrea), "�̳мӳ�" },
            {nameof(AttributeEnum.ExpIncrea), "����ӳ�" },
            {nameof(AttributeEnum.BurstIncrea), "���ʼӳ�" },
            {nameof(AttributeEnum.GoldIncrea), "��Ҽӳ�" },
            {nameof(AttributeEnum.SecondExp), "�ݵ㾭��" },
            {nameof(AttributeEnum.RestoreHp), "ÿ���Ѫ" },
            {nameof(AttributeEnum.RestoreHpPercent), "ÿ���Ѫ����" },
        };
    }
}
