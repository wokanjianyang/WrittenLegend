using Sirenix.OdinInspector;

namespace Game
{
    public enum AttributeEnum
    {
        SkillDamage = -6,
        Color = -5,
        Name = -4,
        Level = -3,
        Exp = -2, //经验值
        Power = -1, //战力
        CurrentHp = 0, //当前生命
        HP = 1, //生命值
        PhyAtt = 2, //物理攻击
        MagicAtt = 3,//魔法攻击
        SpiritAtt = 4, //道术攻击
        Def = 5, //防御
        Speed = 6, //攻速
        Lucky = 7, //幸运
        CritRate = 8, //暴击率
        CritDamage = 9, //暴害增加
        CritRateResist = 10, //抗暴
        CritDamageResist = 11, //爆伤减免
        DamageIncrea = 12, //伤害增加
        DamageResist = 13, //伤害减少
        AttIncrea = 14, //攻击加成
        HpIncrea = 15, //生命加成
        DefIncrea = 16,//防御加成
        InheritIncrea = 17, //继承加成
        ExpIncrea = 18, //经验加成
        BurstIncrea = 19, //爆率加成
        GoldIncrea = 20, //金币加成
        SecondExp = 21, //泡点经验
        RestoreHp = 22, //固定回血数值
        RestoreHpPercent = 23,//百分比回血数值
        WarriorSkillPercent = 41, //战士技能百分比系数
        WarriorSkillDamage = 42, //战士技能固定系数
        MageSkillPercent = 43, //法师技能百分比系数
        MageSkillDamage = 44, //法师技能固定系数
        WarlockSkillPercent = 45, //道士技能百分比系数
        WarlockSkillDamage = 46, //道士技能固定系数
    }

    /// <summary>
    /// 属性来源
    /// </summary>
    public enum AttributeFrom
    {
        HeroPanel = 0, //人物面板总属性
        HeroBase = 1, //人物升级属性
        EquipBase = 2, //装备基础属性
        EquiStrong = 3, //装备强化属性
        Skill = 4,//技能增幅
        Tower = 5,//无尽塔
        /// <summary>
        /// 测试属性
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
        [LabelText("武器")]
        武器 = 1,
        [LabelText("衣服")]
        衣服 = 2,
        [LabelText("项链")]
        项链 = 3,
        [LabelText("头盔")]
        头盔 = 4,
        [LabelText("左手镯")]
        左手镯 = 5,
        [LabelText("右手镯")]
        右手镯 = 6,
        [LabelText("左戒指")]
        左戒指 = 7,
        [LabelText("右戒指")]
        右戒指 = 8,
        [LabelText("腰带")]
        腰带 = 9,
        [LabelText("鞋子")]
        鞋子 = 10,

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

    public enum RoleType {
        Warrior =1, //战士
        Mage = 2, //法师
        Warlock = 3, //道士
    }
}
