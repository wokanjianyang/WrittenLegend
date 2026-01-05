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
        Accuracy = 32, //命中
        PhyDamage = 33, //物伤加成
        MagicDamage = 34,//魔伤加成
        SpiritDamage = 35, //道伤加成
        InheritAdvance = 36, //高级继承
        Protect = 37,//免疫
        BurstMul = 38,//连爆
        Miss2 = 39,//二次闪避
        Strong = 40,//韧性

        WarriorSkillPercent = 41, //战士技能百分比系数
        WarriorSkillDamage = 42, //战士技能固定系数
        MageSkillPercent = 43, //法师技能百分比系数
        MageSkillDamage = 44, //法师技能固定系数
        WarlockSkillPercent = 45, //道士技能百分比系数
        WarlockSkillDamage = 46, //道士技能固定系数

        ExpFinal = 50, //经验增幅
        GoldFinal = 51, //金币增幅
        BurstFinal = 52, //爆率增幅
        QualityFinal = 53, //品质增幅
        CritFinal = 54,//暴击增幅
        LuckyFinal = 55,//幸运增幅
        CritDamageFinal = 56, //爆伤增幅

        Parry = 60,//格挡


        MythAttr = 91,  //神话攻击加成
        MythDef = 92,  //神话防御加成
        MythHp = 93,  //神话生命加成
        MythAll = 94, //神话全属性
        SpiritAll = 95,//英灵全属性
        EquipBaseIncrea = 101, //装备基础属性百分比
        EquipRandomIncrea = 102, //装备随机属性百分比
        EquipStrengthIncrea = 103, //装备强化属性百分比

        MonsterFaster = 107,//副刷新速度加快
        DropFinal = 108, //稀有爆率增幅
        MetailFinal = 109, //挖矿
        SoulPercent = 110, //炼魂夺魄
        Talent = 111, //天赋点
        DefendRate = 112, //防御系数
        SpRate = 113,//护盾固防
        RealHpDamage = 114,//真实血量伤害
        RealMulDamageResist = 115, //溢出减伤迭代计算
        RealCritRate = 116,//溢出暴击转为伤害加成
        LuckyHit = 117, //青龙之力-幸运一击
        Relic2 = 118, //神器2
        Relic3 = 119, //神器2
        Relic4 = 120, //神器2
        Relic5 = 121, //神器2
        RelicRise = 122, //所有神器等级+1


        AurasDamageResist = 201, //光环减伤
        AurasDamageIncrea = 202, //光环增伤
        AurasAttrIncrea = 203,//

        SkillUpCount = 301, //技能栏出战数量
        SkillPhyDamage = 302, //物理伤害
        SkillMagicDamage = 303,//魔法伤害
        SkillSpiritDamage = 304, //道术伤害
        SkillAllDamage = 305, //所有伤害加成
        SkillValetCount = 306, //召唤数量+1
        SkillValetSpeed = 307, //攻击速度
        SkillValetHp = 308, //生命加成

        SkillDivine2010 = 12010, //分身神技
        SkillDivine3010 = 13010, //无极神技

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
        StrongMul = 2012,//韧性倍率
        Shatter = 2013,//破韧倍率
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
        Legacy = 18,//传世
        Ring = 19,//特戒
        Cycle = 20,//转生
        Pill = 21,//修炼
        SoulBone = 22, //魂骨
        Talent = 23, //天赋
        EquipReform = 24, //改造
        Pet = 25,//宠物
        Relic = 26,//神器
        Stone = 27,//宝石
        Pill2 = 28,
        CardSpeical = 29,//暗金图鉴
        FashionSpeical = 30,//暗金时装
        Pill3 = 31,
        PetSpeical = 32,
        Shengxiao = 33,
        Festive = 34,
        Spirit = 35,

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

        [LabelText("金武器")]
        金武器 = 21,
        [LabelText("金衣服")]
        金衣服 = 22,
        [LabelText("金项链")]
        金项链 = 23,
        [LabelText("金头盔")]
        金头盔 = 24,
        [LabelText("金左镯")]
        金左镯 = 25,
        [LabelText("金右镯")]
        金右镯 = 26,
        [LabelText("金左戒")]
        金左戒 = 27,
        [LabelText("金右戒")]
        金右戒 = 28,
        [LabelText("金腰带")]
        金腰带 = 29,
        [LabelText("金鞋子")]
        金鞋子 = 30,

        [LabelText("暗金武器")]
        暗金武器 = 31,
        [LabelText("暗金衣服")]
        暗金衣服 = 32,
        [LabelText("暗金项链")]
        暗金项链 = 33,
        [LabelText("暗金头盔")]
        暗金头盔 = 34,
        [LabelText("暗金左镯")]
        暗金左镯 = 35,
        [LabelText("暗金右镯")]
        暗金右镯 = 36,
        [LabelText("暗金左戒")]
        暗金左戒 = 37,
        [LabelText("暗金右戒")]
        暗金右戒 = 38,
        [LabelText("暗金腰带")]
        暗金腰带 = 39,
        [LabelText("暗金鞋子")]
        暗金鞋子 = 40,

        混沌武器 = 41,
        混沌衣服 = 42,
        混沌项链 = 43,
        混沌头盔 = 44,
        混沌左镯 = 45,
        混沌右镯 = 46,
        混沌左戒 = 47,
        混沌右戒 = 48,
        混沌腰带 = 49,
        混沌鞋子 = 50,

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
        Infinite = 8,
        Legacy = 9,
        Pill,
        Babel,
        Myth,
    }

    public enum RoleType
    {
        Warrior = 1, //战士
        Mage = 2, //法师
        Warlock = 3, //道士
        All = 99,
    }
}
