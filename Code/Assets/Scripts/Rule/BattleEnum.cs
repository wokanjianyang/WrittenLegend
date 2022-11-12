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
        FrontRow = 0,
        Cross,
        Square,
        Diamond,
        FullBox
    }
}
