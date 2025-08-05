namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class RecoverySettingNew
    {
        //普通装备
        public int EquipQualityKeep { get; set; } = 0;
        public int GoldTotal { get; set; } = 0;

        public int ExpTotal { get; set; } = 0;

        public int LuckyTotal { get; set; } = 0;

        public int DropRate { get; set; } = 0;

        public int DropQuality { get; set; } = 0;

        public int EquipQualityRecovery { get; set; } = 0;

        public int EquipLevel { get; set; } = 0;

        public Dictionary<int, bool> EquipRole { get; private set; } = new Dictionary<int, bool>();

        //红色装备
        public bool RedRecovery { get; set; } = false;

        public bool RedKeep { get; set; } = false;

        public int RedExpTotal { get; set; } = 0;

        public int RedGoldTotal { get; set; } = 0;

        public int RedDropRate { get; set; } = 0;

        public int RedDropQuality { get; set; } = 0;

        //金色装备
        public bool EquipiGoldenRecovery { get; set; } = false;

        public bool EquipiGoldenKeep { get; set; } = false;

        public int EquipGoldenTotal { get; set; } = 0;

        //暗金装备
        public bool EquipiDarkRecovery { get; set; } = false;

        public bool EquipiDarkKeep { get; set; } = false;

        public int EquipDarkTotal { get; set; } = 0;

        //普通专属
        public int Exclusive_Recovery { get; set; } = 0;
        public int Exclusive_Keep { get; set; } = 0;

        //传奇专属
        public int Exclusive_Recovery_Golden { get; set; } = 0;
        public int Exclusive_Keep_Golden { get; set; } = 0;

        //不朽专属
        public int Exclusive_Recovery_Dark { get; set; } = 0;
        public int Exclusive_Keep_Dark { get; set; } = 0;

        //其他回收

        public int SpecailLevel { get; set; } = 0;

        public int HalidomLevel { get; set; } = 0;

        public int RedStoneLevel { get; set; } = 0;

        public int PetQuality { get; set; } = 0;


        public RecoverySettingNew()
        {

        }

        public bool CheckRecovery(Item item, RecoveryType type)
        {
            if (item.IsLock)
            {
                return false;
            }

            //int qality = item.GetQuality();
            //SkillReserveQuanlity.TryGetValue(qality, out bool rq);

            //if (item.Type == ItemType.Equip)
            //{
            //    Equip equip = item as Equip;
            //    int role = equip.EquipConfig.Role;

            //    if (GoldTotal > 0)
            //    {
            //        long gt = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.GoldIncrea).Select(m => m.Value).Sum();
            //        if (gt >= GoldTotal && rq)
            //        {
            //            item.IsKeep = true;
            //            return false;
            //        }
            //    }

            //    if (ExpTotal > 0)
            //    {
            //        long et = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.ExpIncrea).Select(m => m.Value).Sum();
            //        if (et >= ExpTotal && rq)
            //        {
            //            item.IsKeep = true;
            //            return false;
            //        }
            //    }

            //    if (LuckyTotal > 0)
            //    {
            //        long lucky = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.Lucky).Select(m => m.Value).Sum();
            //        if (lucky >= LuckyTotal && rq)
            //        {
            //            item.IsKeep = true;
            //            return false;
            //        }
            //    }

            //    if (DropRate > 0)
            //    {
            //        long rateTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.BurstIncrea).Select(m => m.Value).Sum();
            //        if (rateTotal >= DropRate && rq)
            //        {
            //            item.IsKeep = true;
            //            return false;
            //        }
            //    }

            //    if (DropQuality > 0)
            //    {
            //        long qualityTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.QualityIncrea).Select(m => m.Value).Sum();
            //        if (qualityTotal >= DropQuality && rq)
            //        {
            //            item.IsKeep = true;
            //            return false;
            //        }
            //    }

            //    if (equip.SkillSuitConfig != null)
            //    {
            //        int c = GameProcessor.Inst.User.SkillList.Where(m => m.SkillId == equip.SkillSuitConfig.SkillId && m.Recovery).Count();
            //        if (c == 1 && item.Level >= EquipLevel && rq)
            //        {
            //            item.IsKeep = true;
            //            return false;
            //        }
            //    }

            //    if (equip.Part <= 10)
            //    {
            //        //if (item.Level < EquipLevel && item.GetQuality() <= 5 && EquipQuanlity.GetValueOrDefault(item.GetQuality(), false)) //如果勾选了橙色，低于等级就回收
            //        //{
            //        //    return true;
            //        //}

            //        if ((EquipQuanlity.GetValueOrDefault(item.GetQuality(), false) || item.Level < EquipLevel || EquipRole.GetValueOrDefault(role, false))
            //            && equip.Quality < 6)
            //        {
            //            return true;
            //        }

            //        if (EquipQuanlity.GetValueOrDefault(6, false) && equip.Quality == 6 && equip.Layer <= 1) //红色回收
            //        {
            //            return true;
            //        }
            //    }

            //    if (equip.Part > 10 && equip.Level < SpecailLevel)
            //    {
            //        return true;
            //    }
            //}
            //else if (item.Type == ItemType.Exclusive)
            //{
            //    ExclusiveItem exclusive = item as ExclusiveItem;

            //    if (exclusive.GetLayer() > 1)
            //    {
            //        return false;
            //    }

            //    if (exclusive.SkillSuitConfig != null)
            //    {
            //        int c = GameProcessor.Inst.User.SkillList.Where(m => m.SkillId == exclusive.SkillSuitConfig.SkillId && m.Recovery).Count();
            //        if (c == 1 && rq)
            //        {
            //            item.IsKeep = true;
            //            return false;
            //        }
            //    }

            //    if (ExclusiveQuanlity.GetValueOrDefault(qality, false))
            //    {
            //        return true;
            //    }
            //}
            //else if (item.Type == ItemType.Halidom && type == RecoveryType.Drop)
            //{
            //    if (item.ConfigId >= 40000051 && item.ConfigId <= 41000000 && item.ItemConfig.UseParam < HalidomLevel)
            //    {
            //        return true;
            //    }
            //}
            //else if (item.Type == ItemType.Material && type == RecoveryType.Drop)
            //{
            //    if (item.ConfigId >= 50000001 && item.ConfigId <= 51000000 && item.ItemConfig.UseParam < RedStoneLevel)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
    }
}
