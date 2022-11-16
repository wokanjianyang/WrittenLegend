namespace Game
{
    public enum AttributeEnum
    {
        Color,
        Name,
        Level,
        Exp, //经验值
        Power, //战力
        HP =1, //生命值
        PhyAtt =2, //物理攻击
        MagicAtt =3,//魔法攻击
        SpiritAtt =4, //道术攻击
        Def =5, //防御
        Speed =6, //攻速
        Lucky =7, //幸运
        CritRate = 8, //暴击率
        CritDamage = 9, //暴害增加
        CritRateResist =10, //抗暴
        CritDamageResist = 11, //爆伤减免
        DamageIncrea =12, //伤害增加
        DamageResist = 13, //伤害减少
        AttIncrea = 14, //攻击加成
        HpIncrea = 15, //生命加成
        InheritIncrea = 16, //继承加成
        ExpIncrea = 17, //经验加成
        BurstIncrea = 18, //爆率加成

    }
    
    public enum PlayerType
    {
        Hero = 0,
        Enemy
    }
}
