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
        public long Level { get; set; }
    }


    public class SetPlayerHPEvent : SDD.Events.Event
    {
    }

    public class ShowMsgEvent : SDD.Events.Event
    {
        public int TargetId { get; set; }
        public MsgType Type { get; set; }
        public string Content { get; set; }
    }

    public class ShowGameMsgEvent : SDD.Events.Event
    {
        public Transform Parent { get; set; }
        public string Content { get; set; }

        public ToastTypeEnum ToastType { get; set; } = ToastTypeEnum.Normal;
    }

    public class CheckGameCheatEvent : SDD.Events.Event
    {

    }

    public class PlayerDeadEvent : SDD.Events.Event
    {
        public int RoundNum { get; set; }
    }

    public class HeroChangeEvent : SDD.Events.Event {
        public UserChangeType Type { get; set; }
    }

    public class HeroUseEquipEvent : SDD.Events.Event
    {
    }
    public class HeroUnUseEquipEvent : SDD.Events.Event
    {
    }
    public class DeadRewarddEvent : SDD.Events.Event
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
    }

    public class UserInfoUpdateEvent : SDD.Events.Event
    {
    }

    public class HeroLevelUp : SDD.Events.Event
    {

    }

    public class UserAttrChangeEvent : SDD.Events.Event
    {

    }

    public class ActiveAchievementEvent : SDD.Events.Event { 
        public int Id { get; set; }
    }
    public class HeroAttrChangeEvent : SDD.Events.Event
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
    public class HideAttackIcon : SDD.Events.Event
    {
        public RoundType RoundType { get; set; }
    }
    public class HeroUseSkillBookEvent : SDD.Events.Event
    {
        public bool IsLearn { get; set; }

        public BoxItem BoxItem { get; set; }

        public long Quantity { get; set; }
    }

    //选择出战技能
    public class HeroUpdateSkillEvent : SDD.Events.Event
    {
        public SkillPanel SkillPanel { get; set; }
    }
    public class HeroUpdateAllSkillEvent : SDD.Events.Event
    {
    }
}
