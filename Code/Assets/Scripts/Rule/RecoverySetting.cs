namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class RecoverySetting
    {
        public Dictionary<int, bool> EquipQuanlity { get; private set; } = new Dictionary<int, bool>();
        public Dictionary<int, bool> ExclusiveQuanlity { get; private set; } = new Dictionary<int, bool>();

        public int EquipLevel { get; set; } = 0;

        public int SpecailLevel { get; set; } = 0;

        public int GoldTotal { get; set; } = 0;

        public int ExpTotal { get; set; } = 0;

        public int LuckyTotal { get; set; } = 0;

        public int DropRate { get; set; } = 0;

        public int DropQuality { get; set; } = 0;

        public Dictionary<int, bool> EquipRole { get; private set; } = new Dictionary<int, bool>();
        public int SkillBookLevel { get; set; } = 0;//

        public RecoverySetting()
        {
        }

        public void SetEquipQuanlity(int quanlity, bool check)
        {
            this.EquipQuanlity[quanlity] = check;
        }

        public void SetExclusiveQuanlity(int quanlity, bool check)
        {
            this.ExclusiveQuanlity[quanlity] = check;
        }

        public void SetEquipRole(int role, bool check)
        {
            this.EquipRole[role] = check;
        }


        public bool CheckRecovery(Item item)
        {
            if (item.IsLock)
            {
                return false;
            }

            if (item.Type == ItemType.Equip)
            {
                Equip equip = item as Equip;
                int role = equip.EquipConfig.Role;

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
                    int c = GameProcessor.Inst.User.SkillList.Where(m => m.SkillId == equip.SkillSuitConfig.SkillId && m.Recovery).Count();
                    if (c == 1 && item.Level >= EquipLevel)
                    {
                        item.IsKeep = true;
                        return false;
                    }
                }

                if (equip.Part <= 10)
                {
                    if (item.Level < EquipLevel && EquipQuanlity.GetValueOrDefault(5, false)) //如果勾选了橙色，低于等级就回收
                    {
                        return true;
                    }

                    if ((EquipQuanlity.GetValueOrDefault(item.GetQuality(), false) || item.Level < EquipLevel || EquipRole.GetValueOrDefault(role, false))
                        && equip.Quality < 5)
                    {
                        return true;
                    }
                }

                if (equip.Part > 10 && equip.Level < SpecailLevel)
                {
                    return true;
                }
            }
            else if (item.Type == ItemType.Exclusive)
            {
                if (ExclusiveQuanlity.GetValueOrDefault(item.GetQuality(), false))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
