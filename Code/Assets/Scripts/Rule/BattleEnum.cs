using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    public enum RoundType
    {
        Hero = 0,
        Monster 
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
        Survivors
    }

    public enum ComponentOrder
    {
        PlayerManager = 0,
        BattleRule,
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
        Cross =1,
        /// <summary>
        /// 矩形
        /// </summary>
        Square =2,
        /// <summary>
        /// 菱形
        /// </summary>
        Diamond =3,
        /// <summary>
        /// 全图
        /// </summary>
        FullBox = 4
    }

    public enum SkillCenter { 
        Self ,
        Enemy
    }
}
