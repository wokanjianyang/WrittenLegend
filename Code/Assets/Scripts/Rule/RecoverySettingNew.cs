namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

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

        //混沌装备
        public int Equip_Hundun_Recovery { get; set; } = 0;
        public bool Equip_Hundun_Keep { get; set; } = false;

        public int Equip_Hundun_Total { get; set; } = 0;

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

        public int ShengxiaoQuality { get; set; } = 0;

        private int KeepStartQuality = 3;

        public RecoverySettingNew()
        {

        }

        public bool CheckRecovery(Item item, RecoveryType type)
        {
            if (item.IsLock)
            {
                return false;
            }

            if (item.Type == ItemType.Equip)
            {
                Equip equip = item as Equip;

                if (equip.Layer > 1)
                {
                    return false;
                }

                int role = equip.EquipConfig.Role;
                int cycle = equip.EquipConfig.Cycle;
                int level = equip.Level;
                int quality = equip.GetQuality();
                long ar = equip.GetAttrRateCount();
                bool keepSkill = false;
                if (equip.SkillSuitConfig != null)
                {
                    keepSkill = GameProcessor.Inst.User.CheckKeepSkill(equip.SkillRuneConfig.SkillId, equip.SkillRuneConfig.SkillLayer);
                }

                if (cycle == 0)
                {
                    //四格回收
                    if (cycle == 0 && level < SpecailLevel)
                    {
                        return true;
                    }
                }
                else if (cycle == 1)
                {
                    //普通回收

                    //先判断保留
                    if (EquipQualityKeep > 0 && quality >= EquipQualityKeep + KeepStartQuality)
                    {
                        if (GoldTotal > 0)
                        {
                            long gt = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.GoldIncrea).Select(m => m.Value).Sum();
                            if (gt >= GoldTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (ExpTotal > 0)
                        {
                            long et = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.ExpIncrea).Select(m => m.Value).Sum();
                            if (et >= ExpTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (LuckyTotal > 0)
                        {
                            long lucky = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.Lucky).Select(m => m.Value).Sum();
                            if (lucky >= LuckyTotal)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (DropRate > 0)
                        {
                            long rateTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.BurstIncrea).Select(m => m.Value).Sum();
                            if (rateTotal >= DropRate)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (DropQuality > 0)
                        {
                            long qualityTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.QualityIncrea).Select(m => m.Value).Sum();
                            if (qualityTotal >= DropQuality)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }

                        if (equip.SkillSuitConfig != null)
                        {
                            if (item.Level >= EquipLevel && keepSkill)
                            {
                                item.IsKeep = true;
                                return false;
                            }
                        }
                    }

                    if (equip.Level < EquipLevel || EquipRole.GetValueOrDefault(role, false) || quality <= EquipQualityRecovery)
                    {
                        return true;
                    }
                }
                else if (cycle == 2)
                {
                    if (!RedRecovery)
                    {
                        return false;
                    }

                    //红装回收
                    if (RedGoldTotal > 0)
                    {
                        long gt = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.GoldIncrea).Select(m => m.Value).Count();
                        if (gt >= RedGoldTotal)
                        {
                            item.IsKeep = true;
                            return false;
                        }
                    }

                    if (RedExpTotal > 0)
                    {
                        long et = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.ExpIncrea).Select(m => m.Value).Count();
                        if (et >= RedExpTotal)
                        {
                            item.IsKeep = true;
                            return false;
                        }
                    }

                    if (RedDropRate > 0)
                    {
                        long rateTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.BurstIncrea).Select(m => m.Value).Count();
                        if (rateTotal >= RedDropRate)
                        {
                            item.IsKeep = true;
                            return false;
                        }
                    }

                    if (RedDropQuality > 0)
                    {
                        long qualityTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.QualityIncrea).Select(m => m.Value).Count();
                        if (qualityTotal >= RedDropQuality)
                        {
                            item.IsKeep = true;
                            return false;
                        }
                    }

                    if (keepSkill && RedKeep)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    return true;
                }
                else if (cycle == 3)
                {
                    if (!EquipiGoldenRecovery)
                    {
                        return false;
                    }

                    //金装回收
                    if (EquipGoldenTotal > 0 && equip.GetAttrRateCount() >= EquipGoldenTotal)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    if (keepSkill && EquipiGoldenKeep)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    return true;
                }
                else if (cycle == 4)
                {
                    if (!EquipiDarkRecovery)
                    {
                        return false;
                    }

                    //暗金回收
                    if (EquipDarkTotal > 0 && equip.GetAttrRateCount() >= EquipDarkTotal)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    if (keepSkill && EquipiDarkKeep)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    return true;
                }
                else if (cycle == 5)
                {
                    if (Equip_Hundun_Total > 0 && equip.GetAttrRateCount() >= Equip_Hundun_Total)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    if (keepSkill && Equip_Hundun_Keep)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    if (quality <= Equip_Hundun_Recovery)
                    {
                        return true;
                    }
                }
            }
            else if (item.Type == ItemType.Exclusive)
            {
                ExclusiveItem exclusive = item as ExclusiveItem;

                if (exclusive.GetLayer() > 1 || exclusive.GetLevel() > 1)
                {
                    return false;
                }

                int cycle = exclusive.ExclusiveConfig.Cycle;
                int quality = exclusive.GetQuality();

                bool keepSkill = false;
                if (exclusive.SkillSuitConfig != null)
                {
                    keepSkill = GameProcessor.Inst.User.CheckKeepSkill(exclusive.SkillSuitConfig.SkillId, exclusive.SkillSuitConfig.SkillLayer);
                }

                if (cycle == 1)
                {
                    if (Exclusive_Keep > 0 && quality >= Exclusive_Keep + KeepStartQuality && keepSkill)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    if (Exclusive_Recovery > 0 && quality <= Exclusive_Recovery)
                    {
                        return true;
                    }
                }
                else if (cycle == 2)
                {
                    if (Exclusive_Keep_Golden > 0 && quality >= Exclusive_Keep_Golden + KeepStartQuality && keepSkill)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    if (Exclusive_Recovery_Golden > 0 && quality <= Exclusive_Recovery_Golden)
                    {
                        return true;
                    }
                }
                else if (cycle == 3)
                {
                    if (Exclusive_Keep_Dark > 0 && quality >= Exclusive_Keep_Dark + KeepStartQuality && keepSkill)
                    {
                        item.IsKeep = true;
                        return false;
                    }

                    if (Exclusive_Recovery_Dark > 0 && quality <= Exclusive_Recovery_Dark)
                    {
                        return true;
                    }
                }
            }
            else if (item.Type == ItemType.Halidom && type == RecoveryType.Drop)
            {
                if (item.ConfigId >= 40000051 && item.ConfigId <= 41000000 && item.ItemConfig.UseParam < HalidomLevel)
                {
                    return true;
                }
            }
            else if (item.Type == ItemType.Material && type == RecoveryType.Drop)
            {
                if (item.ConfigId >= 50000001 && item.ConfigId <= 51000000 && item.ItemConfig.UseParam < RedStoneLevel)
                {
                    return true;
                }
            }
            else if (item.Type == ItemType.Pet)
            {
                Pet pet = item as Pet;
                if (item.GetQuality() <= PetQuality && pet.PetLayer.Data == 1 && pet.PetLevel.Data == 1)
                {
                    return true;
                }
            }
            else if (item.Type == ItemType.Shengxiao)
            {
                Shengxiao shengxaio = item as Shengxiao;
                if (item.GetQuality() <= ShengxiaoQuality && shengxaio.LayerData.Data < 1 && shengxaio.LevelData.Data < 1)
                {
                    return true;
                }
            }

            return false;
        }
    }

    public enum RecoveryType
    {
        Drop = 1,//打怪掉落
        Other = 2, //其他
    }
}
