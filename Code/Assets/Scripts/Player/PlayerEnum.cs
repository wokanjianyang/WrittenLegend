using Sirenix.OdinInspector;

namespace Game
{
    public enum AttributeEnum
    {
        Color = -5,
        Name = -4,
        Level = -3,
        Exp = -2, //����ֵ
        Power = -1, //ս��
        HP =1, //����ֵ
        PhyAtt =2, //������
        MagicAtt =3,//ħ������
        SpiritAtt =4, //��������
        Def =5, //����
        Speed =6, //����
        Lucky =7, //����
        CritRate = 8, //������
        CritDamage = 9, //��������
        CritRateResist =10, //����
        CritDamageResist = 11, //���˼���
        DamageIncrea =12, //�˺�����
        DamageResist = 13, //�˺�����
        AttIncrea = 14, //�����ӳ�
        HpIncrea = 15, //�����ӳ�
        DefIncrea = 16,
        InheritIncrea = 17, //�̳мӳ�
        ExpIncrea = 18, //����ӳ�
        BurstIncrea = 19, //���ʼӳ�

    }

    /// <summary>
    /// ������Դ
    /// </summary>
    public enum AttributeFrom
    {
        HeroBase =1, //������������
        EquipBase =2 , //װ����������
        EquiStrong =3, //װ��ǿ������
        /// <summary>
        /// ��������
        /// </summary>
        Test=99,
    }

    public enum PlayerType
    {
        Hero = 0,
        Enemy,
        Valet, 
    }

    public enum SlotType
    {
        [LabelText("����")]
        ���� = 2,
        [LabelText("�·�")]
        �·� = 1,
        [LabelText("ͷ��")]
        ͷ�� = 4,
        [LabelText("����")]
        ���� = 3,
        [LabelText("����")]
        ���� = 6,
        [LabelText("��ָ")]
        ��ָ = 5,
        [LabelText("Ь��")]
        Ь�� = 8,
        [LabelText("����")]
        ���� = 7
    }
    public enum ProgressType
    {
        [LabelText("��ɫ����")]
        PlayerExp = 0,

        [LabelText("���ܾ���")]
        SkillExp = 1,

        [LabelText("��ɫ����")]
        PlayerHP = 2,
    }
}
