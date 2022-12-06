using UnityEngine;

namespace Game
{
    public class SetBackgroundColorEvent : SDD.Events.Event
    {
        public Color Color { get; set; }
    }
    
    public class SetPlayerNameEvent : SDD.Events.Event
    {
        public string Name { get; set; }
    }
    
    public class SetPlayerLevelEvent : SDD.Events.Event
    {
        public string Level { get; set; }
    }
    
    
    public class SetPlayerHPEvent : SDD.Events.Event
    {
        public string HP { get; set; }
    }
    
    public class ShowMsgEvent : SDD.Events.Event
    {
        public int TargetId { get; set; }
        public string Content { get; set; }
    }

    public class PlayerDeadEvent : SDD.Events.Event
    {
        public int RoundNum { get; set; }
    }

    public class HeroChangeEvent : SDD.Events.Event { 
        public Hero.HeroChangeType Type { get; set; } 
    }

    public class HeroUseEquipEvent : SDD.Events.Event
    {
        public int Position { get; set; } 
        public Equip Equip { get; set; }
    }

    public class DeadRewarddEvent : SDD.Events.Event
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
    }

    public class HeroInfoUpdateEvent : SDD.Events.Event
    {
    }
    public class HeroBagUpdateEvent : SDD.Events.Event
    {
    }
}
