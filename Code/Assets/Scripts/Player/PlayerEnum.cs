using Sirenix.OdinInspector;

namespace Game
{
    public enum AttributeEnum
    {
        SkillDamage = -6,
        Color = -5,
        Name = -4,
        Level = -3,
        Exp = -2, //����ֵ
        Power = -1, //ս��
        CurrentHp = 0, //��ǰ����
        HP = 1, //����ֵ
        PhyAtt = 2, //������
        MagicAtt = 3,//ħ������
        SpiritAtt = 4, //��������
        Def = 5, //����
        Speed = 6, //����
        Lucky = 7, //����
        CritRate = 8, //������
        CritDamage = 9, //��������
        CritRateResist = 10, //����
        CritDamageResist = 11, //���˼���
        DamageIncrea = 12, //�˺�����
        DamageResist = 13, //�˺�����
        AttIncrea = 14, //�����ӳ�
        HpIncrea = 15, //�����ӳ�
        DefIncrea = 16,//�����ӳ�
        InheritIncrea = 17, //�̳мӳ�
        ExpIncrea = 18, //����ӳ�
        BurstIncrea = 19, //���ʼӳ�
        GoldIncrea = 20, //��Ҽӳ�
        SecondExp = 21, //�ݵ㾭��
        RestoreHp = 22, //�̶���Ѫ��ֵ
        RestoreHpPercent = 23,//�ٷֱȻ�Ѫ��ֵ
        WarriorSkillPercent = 41, //սʿ���ܰٷֱ�ϵ��
        WarriorSkillDamage = 42, //սʿ���̶ܹ�ϵ��
        MageSkillPercent = 43, //��ʦ���ܰٷֱ�ϵ��
        MageSkillDamage = 44, //��ʦ���̶ܹ�ϵ��
        WarlockSkillPercent = 45, //��ʿ���ܰٷֱ�ϵ��
        WarlockSkillDamage = 46, //��ʿ���̶ܹ�ϵ��
    }

    /// <summary>
    /// ������Դ
    /// </summary>
    public enum AttributeFrom
    {
        HeroPanel = 0, //�������������
        HeroBase = 1, //������������
        EquipBase = 2, //װ����������
        EquiStrong = 3, //װ��ǿ������
        Skill = 4,//��������
        Tower = 5,//�޾���
        /// <summary>
        /// ��������
        /// </summary>
        Test = 99,
    }

    public enum PlayerType
    {
        Hero = 0,
        Enemy,
        Valet, 
    }

    public enum MondelType { 
        Nomal = 1,
        Boss = 2,
    }

    public enum SlotType
    {
        [LabelText("����")]
        ���� = 1,
        [LabelText("�·�")]
        �·� = 2,
        [LabelText("����")]
        ���� = 3,
        [LabelText("ͷ��")]
        ͷ�� = 4,
        [LabelText("������")]
        ������ = 5,
        [LabelText("������")]
        ������ = 6,
        [LabelText("���ָ")]
        ���ָ = 7,
        [LabelText("�ҽ�ָ")]
        �ҽ�ָ = 8,
        [LabelText("����")]
        ���� = 9,
        [LabelText("Ь��")]
        Ь�� = 10,

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

    public enum RoleType {
        Warrior =1, //սʿ
        Mage = 2, //��ʦ
        Warlock = 3, //��ʿ
    }
}
