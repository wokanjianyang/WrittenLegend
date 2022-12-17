using Sirenix.OdinInspector;

namespace Game
{
    public enum AttributeEnum
    {
        Color = -5,
        Name = -4,
        Level = -3,
        Exp = -2, //经验值
        Power = -1, //战力
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
        DefIncrea = 16,
        InheritIncrea = 17, //继承加成
        ExpIncrea = 18, //经验加成
        BurstIncrea = 19, //爆率加成

    }

    /// <summary>
    /// 属性来源
    /// </summary>
    public enum AttributeFrom
    {
        HeroBase =1, //人物升级属性
        EquipBase =2 , //装备基础属性
        EquiStrong =3, //装备强化属性
        /// <summary>
        /// 测试属性
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
        [LabelText("武器")]
        武器 = 2,
        [LabelText("衣服")]
        衣服 = 1,
        [LabelText("头盔")]
        头盔 = 4,
        [LabelText("项链")]
        项链 = 3,
        [LabelText("手镯")]
        手镯 = 6,
        [LabelText("戒指")]
        戒指 = 5,
        [LabelText("鞋子")]
        鞋子 = 8,
        [LabelText("腰带")]
        腰带 = 7
    }
    public enum ProgressType
    {
        [LabelText("角色经验")]
        PlayerExp = 0,

        [LabelText("技能经验")]
        SkillExp = 1,

        [LabelText("角色经验")]
        PlayerHP = 2,
    }
}
