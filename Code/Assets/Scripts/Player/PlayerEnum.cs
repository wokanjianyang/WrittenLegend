namespace Game
{
    public enum AttributeEnum
    {
        Color,
        Name,
        Level,
        Exp, //����ֵ
        Power, //ս��
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
        InheritIncrea = 16, //�̳мӳ�
        ExpIncrea = 17, //����ӳ�
        BurstIncrea = 18, //���ʼӳ�

    }
    
    public enum PlayerType
    {
        Hero = 0,
        Enemy
    }
}
