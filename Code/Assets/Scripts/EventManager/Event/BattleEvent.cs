using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChangePageEvent : SDD.Events.Event
    {
        public ViewPageType Page { get; set; }
    }

    public class ShowEquipDetailEvent : SDD.Events.Event
    {
        //public Vector3 Position { get; set; }

        public BoxItem boxItem { get; set; }

        public int EquipPosition { get; set; }
    }

    public class EquipOneEvent : SDD.Events.Event
    {
        public bool IsWear { get; set; } = true;

        public int Part { get; set; }
        public BoxItem BoxItem { get; set; }
    }


    public class BattleMsgEvent : SDD.Events.Event
    {
        public string Message { get; set; }
        //public int RoundNum { get; set; }
        //public int MonsterId { get; set; }

        //public long Exp { get; set; }
        //public long Gold { get; set; }

        //public List<Item> Drops { get; set; }

        //public MsgType MsgType { get; set; } = MsgType.Drop;
        public BattleType BattleType { get; set; } = BattleType.Normal;
    }

    public class BattlePhantomMsgEvent : SDD.Events.Event
    {
        public string Message { get; set; }
    }

    public class SkillBookLearnEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
    }

    public class RecoveryEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
    }
    public class ForgingEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
    }
    public class EquipLockEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
        public bool IsLock { get; set; }
    }
    public class AutoRecoveryEvent : SDD.Events.Event
    {
    }
    public class BagUseEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }

        public int Quantity { get; set; }
    }

    public class ShowTowerWindowEvent : SDD.Events.Event
    {

    }

    public class UpdateTowerWindowEvent : SDD.Events.Event
    {

    }

    public class DialogSettingEvent : SDD.Events.Event
    {
        public bool IsOpen { get; set; }
    }
    public class BossInfoEvent : SDD.Events.Event
    {

    }

    public class PhantomEvent : SDD.Events.Event
    {


    }
    public class PhantomStartEvent : SDD.Events.Event
    {
        public int PhantomId { get; set; }
    }

    public class PhantomEndEvent : SDD.Events.Event
    {
        public int PhantomId { get; set; }
    }

    public class ShowPhantomInfoEvent : SDD.Events.Event
    {
        public int Time { get; set; }
    }

    public class ShowBossFamilyInfoEvent : SDD.Events.Event
    {
        public int Count { get; set; }
    }
    public class BossFamilyStartEvent : SDD.Events.Event
    {

    }
    public class BossFamilyEndEvent : SDD.Events.Event
    {

    }


    public class ChangeMapEvent : SDD.Events.Event
    {
        public int MapId { get; set; }
    }

    public class ChangeFloorEvent : SDD.Events.Event
    {
        public int Floor { get; set; }
    }

    public class EquipStrengthSelectEvent : SDD.Events.Event
    {
        public int Position { get; set; }
    }
    public class EquipRefineSelectEvent : SDD.Events.Event
    {
        public int Position { get; set; }
    }

    public class ChangeCompositeTypeEvent : SDD.Events.Event
    {
        public string CompositeType { get; set; }
    }

    public class CompositeEvent : SDD.Events.Event
    {
        public CompositeConfig Config { get; set; }
    }
    public class CompositeUIFreshEvent : SDD.Events.Event
    {
    }

    public class MaterialUseEvent : SDD.Events.Event
    {
        public int MaterialId { get; set; }
        public long Quantity { get; set; }
    }

    public class StartCopyEvent : SDD.Events.Event
    {
        public int Rate { get; set; }
        public int MapId { get; set; }
    }

    public class AutoStartCopyEvent : SDD.Events.Event
    {

    }

    public class EndCopyEvent : SDD.Events.Event
    {
        public int MapId { get; set; }
    }

    public class ShowCopyInfoEvent : SDD.Events.Event
    {
        public int Mc1 { get; set; }
        public int Mc2 { get; set; }
        public int Mc3 { get; set; }
        public int Mc4 { get; set; }
        public int Mc5 { get; set; }
    }

    public class TaskChangeEvent : SDD.Events.Event
    {

    }

    public class BattleLoseEvent : SDD.Events.Event
    {
        public long Time { get; set; }
    }

    public class SecondaryConfirmationEvent : SDD.Events.Event
    {

    }

    public class SecondaryConfirmTextEvent : SDD.Events.Event
    {
        public string Text { get; set; }
    }

    public class SecondaryConfirmCloseEvent : SDD.Events.Event
    {

    }

    public class ShowSoulRingEvent : SDD.Events.Event
    {

    }

    public class ShowAchievementEvent : SDD.Events.Event
    {

    }

    public class ShowExclusiveEvent : SDD.Events.Event
    {
    }

    public class ShowSelectEvent : SDD.Events.Event
    {
        public BoxItem boxItem { get; set; }
    }

    public class SelectGiftEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
        public Item Item { get; set; }
    }
}