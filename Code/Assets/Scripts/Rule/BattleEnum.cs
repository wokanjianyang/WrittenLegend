using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public enum RoundType
    {
        Hero = 0,
        Monster,
        Valet
    }

    public enum PlayerActionType
    {
        None = 0,
        WaitingInput,
        InputEnd
    }

    public enum RuleType
    {
        [LabelText("常规")]
        Normal = 0,

        [LabelText("幸存者")]
        Survivors,

        [LabelText("装备副本")]
        EquipCopy,

        Phantom,
        BossFamily,
        AnDian,
        Defend,
        HeroPhantom,
        Mine,
        Infinite,
    }

    public enum ComponentOrder
    {
        PlayerManager = 0,
        BattleRule,
        ViewPage,
        Dialog,
        TopNav,
        Progress,
        Window
    }

    public enum AttackGeometryType
    {
        /// <summary>
        /// 直线
        /// </summary>
        FrontRow = 0,
        /// <summary>
        /// 十字
        /// </summary>
        Cross = 1,
        /// <summary>
        /// 矩形
        /// </summary>
        Square = 2,
        /// <summary>
        /// 菱形
        /// </summary>
        Diamond = 3,
        /// <summary>
        /// 全图
        /// </summary>
        FullBox = 4,
        /// <summary>
        /// 弧线
        /// </summary>
        Arc = 5,

        Chase = 8,
        Circle = 9,
    }

    public enum AttackCastType
    {
        /// <summary>
        /// 范围内部分
        /// </summary>
        Single = 1,
        /// <summary>
        /// 范围内全部
        /// </summary>
        Area = 2,
    }

    public enum SkillCenter
    {
        Self,
        Enemy
    }

    public enum ViewPageType
    {
        View_Bag = 0,
        View_Battle,
        View_Map,
        View_Skill,
        View_Tower,
        View_Forge,
        View_More,
    }

    public enum TouchIgnoreType
    {
        [LabelText("点击空白处时关闭")]
        HideWithTouchEmpty = 0,
        [LabelText("点击按钮关闭")]
        HideWithCloseBtn,
        [LabelText("自动关闭")]
        HideWithAuto
    }

    public enum MsgType
    {
        Normal = 0,
        Damage = 1,
        Restore = 2,
        Crit = 3,
        Effect = 4,
        Other = 5,
        SkillName = 6,
        Miss = 7,
    }

    public enum TaskType
    {
        Tower = 1, //闯关层数
        Equip = 2, //穿戴装备
        Strength = 3, //强化装备
        ToCopy = 4, //挑战副本
        Recovery = 5, //设置回收
        SkillBook = 6, //设置回收
        BindAccount = 7, //绑定帐号
    }
}
