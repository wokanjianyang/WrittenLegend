using System.Collections.Generic;
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
    public class HeroUnUseEquipEvent : SDD.Events.Event
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
        public List<Item> ItemList { get; set; }
    }

    public class ShowAttackIcon : SDD.Events.Event
    {
        public bool NeedShow { get; set; }
    }
    public class HeroUseSkillBookEvent : SDD.Events.Event
    {
        public bool IsLearn { get; set; }

        public Item Item { get; set; }
    }

    //选择出战技能
    public class HeroUpdateSkillEvent : SDD.Events.Event
    {
        public SkillPanel SkillPanel { get; set; }
    }
}
