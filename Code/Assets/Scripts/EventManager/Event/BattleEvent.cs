using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ChangePageEvent : SDD.Events.Event
    {
        public ViewPageType Page { get; set; }
    }

    public class ShowDetailEvent : SDD.Events.Event
    {
        public ComBoxType Type { get; set; }
        public BoxItem boxItem { get; set; }
    }

    public class ShowExclusiveCardEvent : SDD.Events.Event
    {
        public ComBoxType Type { get; set; }
        public BoxItem boxItem { get; set; }

        public int EquipPosition { get; set; }
    }

    public class ShowEquipDetailEvent : SDD.Events.Event
    {
        //public Vector3 Position { get; set; }

        public ComBoxType Type { get; set; }
        public BoxItem boxItem { get; set; }

        public int EquipPosition { get; set; }
    }

    public class ComBoxSelectEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
        public ComBoxType Type { get; set; }
    }

    public class RefershSelectEvent : SDD.Events.Event
    {
        public Equip Equip { get; set; }
    }

    public class GradeSelectEvent : SDD.Events.Event
    {
        public Equip Equip { get; set; }
    }

    public class ComBoxDeselectEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
        public ComBoxType Type { get; set; }

        public int Position { get; set; }
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

        public RuleType Type { get; set; } = RuleType.Normal;
    }

    public class MineMsgEvent : SDD.Events.Event
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

    public class RestoreEvent : SDD.Events.Event
    {
        public BoxItem BoxItem { get; set; }
    }

    public class LoseEvent : SDD.Events.Event
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
        public RuleType RuleType { get; set; }
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

    public class OpenBossFamilyEvent : SDD.Events.Event
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
        public int Level { get; set; }
        public int Rate { get; set; }
    }

    public class BossFamilyEndEvent : SDD.Events.Event
    {

    }

    public class AnDianStartEvent : SDD.Events.Event
    {

    }
    public class ShowAnDianInfoEvent : SDD.Events.Event
    {
        public int Count { get; set; }
    }

    public class AnDianChangeLevel : SDD.Events.Event
    {
        public int Level { get; set; }
    }


    public class AnDianEndEvent : SDD.Events.Event
    {

    }

    public class OpenDefendEvent : SDD.Events.Event
    {
    }

    public class DefendStartEvent : SDD.Events.Event
    {
    }
    public class ShowDefendInfoEvent : SDD.Events.Event
    {
        public long Count { get; set; }
        public long PauseCount { get; set; }
    }

    public class DefendBuffSelectEvent : SDD.Events.Event
    {
        public int Level { get; set; }
        public int Index { get; set; }
    }

    public class DefendEndEvent : SDD.Events.Event
    {

    }

    public class ShowInfiniteInfoEvent : SDD.Events.Event
    {
        public long Count { get; set; }
        public long PauseCount { get; set; }
    }

    public class ChangeMapEvent : SDD.Events.Event
    {
        public int MapId { get; set; }
    }

    public class OpenMineEvent : SDD.Events.Event
    {
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

    public class FestiveUIFreshEvent : SDD.Events.Event
    {
    }

    public class ExchangeEvent : SDD.Events.Event
    {
        public ExchangeConfig Config { get; set; }
    }

    public class ExclusiveDevourEvent : SDD.Events.Event
    {

    }

    public class ExchangeUIFreshEvent : SDD.Events.Event
    {
    }

    public class SystemUseEvent : SDD.Events.Event
    {
        public ItemType Type { get; set; }
        public int ItemId { get; set; }
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

    public class AutoStartBossFamily : SDD.Events.Event
    {

    }

    public class CloseViewMoreEvent : SDD.Events.Event
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
        public RuleType Type { get; set; }
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

    public class ShowFestiveDialogEvent : SDD.Events.Event
    {

    }

    public class ShowDialogUserAttrEvent : SDD.Events.Event
    {

    }

    public class ChangeExclusiveEvent : SDD.Events.Event
    {
        public int Index { get; set; }
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

    //------------Hero Phantom---------------
    public class OpenHeroPhatomEvent : SDD.Events.Event
    {
    }

    public class HeroPhatomStartEvent : SDD.Events.Event
    {
        public int PhantomId { get; set; }
    }

    public class HeroPhatomEndEvent : SDD.Events.Event
    {
        public int PhantomId { get; set; }
    }

    public class ShowHeroPhatomInfoEvent : SDD.Events.Event
    {
        public int Time { get; set; }
    }
    //------Infinite
    public class InfiniteStartEvent : SDD.Events.Event
    {
    }

    //--------Fashion
    public class OpenFashionDialogEvent : SDD.Events.Event
    {
    }
}