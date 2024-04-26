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
        SecondExp = 21, //每秒经验收益
        RestoreHp = 22, //固定回血数值
        RestoreHpPercent = 23,//百分比回血数值
        QualityIncrea = 24,//品质加成
        SecondGold = 25, //每秒金币收益
        PhyAttIncrea = 26, //物攻加成
        MagicAttIncrea = 27, //魔法加成
        SpiritAttIncrea = 28, //道术加成
        MoveSpeed = 29,//移动速度
        DefIgnore = 30,//无视防御
        Miss = 31, //闪避
        Accuracy = 32, //精准
        PhyDamage = 33, //物伤加成
        MagicDamage = 34,//魔伤加成
        SpiritDamage = 35, //道伤加成
        InheritAdvance = 36, //高级继承

        WarriorSkillPercent = 41, //战士技能百分比系数
        WarriorSkillDamage = 42, //战士技能固定系数
        MageSkillPercent = 43, //法师技能百分比系数
        MageSkillDamage = 44, //法师技能固定系数
        WarlockSkillPercent = 45, //道士技能百分比系数
        WarlockSkillDamage = 46, //道士技能固定系数

        AurasDamageResist = 201, //光环减伤
        AurasDamageIncrea = 202, //光环增伤
        AurasAttrIncrea = 203,//

        EquipBaseIncrea = 101, //装备基础属性百分比
        EquipRandomIncrea = 102, //装备随机属性百分比

        SkillPhyDamage = 302, //物理伤害
        SkillMagicDamage = 303,//魔法伤害
        SkillSpiritDamage = 304, //道术伤害
        SkillAllDamage = 305, //所有伤害加成
        SkillValetCount = 306, //召唤数量+1
        SkillValetSpeed = 307, //攻击速度
        SkillValetHp = 308, //生命加成

        ExtraDamage = 401,//额外伤害

        PanelHp = 1001, //面板生命
        PanelPhyAtt = 1002, //面板物攻
        PanelMagicAtt = 1003, //面板魔法
        PanelSpiritAtt = 1004, //面板道术
        PanelDef = 1005, //面板防御
        PanelAtt = 1006,//面板攻击

        MulAttr = 2001,  //攻击倍率
        MulDef = 2002,  //防御倍率
        MulHp = 2003,  //生命倍率
        MulAttrPhy = 2004, //物攻倍率
        MulAttrMagic = 2005,  //法功倍率
        MulAttrSpirit = 2006,  //道术倍率

        MulPhyDamageRise = 2007,
        MulMagicDamageRise = 2008,
        MulSpiritDamageRise = 2009,

        MulDamageIncrea = 2010,  //增伤倍率
        MulDamageResist = 2011, //减伤倍率
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
        Phantom = 6,//幻神
        EquipSuit = 7, //装备套装
        SoulRing = 8, //魂环
        Auras = 9, //光环
        Achivement = 10, //成就
        Exclusive = 11, //专属
        Card = 12,//图鉴
        Wing = 13, //翅膀
        Fashion = 14, //时装
        EquipRed = 15, //装备套装
        Halidom = 16, //遗物
        Metal = 17,//矿石

        Dingzhi = 98,
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
        Defend,
        HeroPhatom,
        Duplication,
    }

    public enum MondelType
    {
        Nomal = 1,
        Boss = 2,
        YueLing = 5,
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
        [LabelText("斗笠")]
        斗笠 = 11,
        [LabelText("护盾")]
        护盾 = 12,
        [LabelText("神符")]
        神符 = 13,
        [LabelText("魔石")]
        魔石 = 14,

        [LabelText("专属1")]
        神圣怒斩 = 15,
        [LabelText("专属2")]
        神圣噬魂 = 16,
        [LabelText("专属3")]
        神圣血饮 = 17,
        [LabelText("专属4")]
        神圣屠龙 = 18,
        [LabelText("专属5")]
        神圣倚天 = 19,
        [LabelText("专属6")]
        神圣命运 = 20,

        [LabelText("主专属")]
        主专属 = 101,
        [LabelText("副专属")]
        副专属 = 102,
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

    public enum CopyType
    {
        [LabelText("装备副本")]
        装备副本 = 1,
        [LabelText("幻影挑战")]
        幻影挑战 = 2,
        [LabelText("BOSS之家")]
        BossFamily = 3,
        [LabelText("未知暗殿")]
        AnDian = 4,
        [LabelText("守卫沙城")]
        Defend = 5,
        HeorPhantom = 6,
        Mine = 7,
        Infinite =8,
    }

    public enum RoleType
    {
        Warrior = 1, //战士
        Mage = 2, //法师
        Warlock = 3, //道士
        All = 99,
    }
}
