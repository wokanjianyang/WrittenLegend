using Sirenix.OdinInspector;

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
}
