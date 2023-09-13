namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class RecoverySetting
    {
        public Dictionary<int, bool> EquipQuanlity { get; private set; } = new Dictionary<int, bool>();  //??????????(1??? 2??? 3??? 4???)
        public int EquipLevel { get; set; } = 0;

        public int GoldTotal { get; set; } = 0;

        public int ExpTotal { get; set; } = 0;

        public int LuckyTotal { get; set; } = 0;

        public int DropRate { get; set; } = 0;

        public int DropQuality { get; set; } = 0;

        public Dictionary<int, bool> EquipRole { get; private set; } = new Dictionary<int, bool>();
        public int SkillBookLevel { get; set; } = 0;//

        public RecoverySetting() {
        }

        public void SetEquipQuanlity(int quanlity, bool check)
        {
            this.EquipQuanlity[quanlity] = check;
        }

        public void SetEquipRole(int role, bool check)
        {
            this.EquipRole[role] = check;
        }


        public bool CheckRecovery(Item item)
        {
            if (item.Type == ItemType.Equip)
            {
                Equip equip = item as Equip;
                int role = equip.EquipConfig.Role;

                if (GoldTotal > 0)
                {
                    long gt = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.GoldIncrea).Select(m => m.Value).Sum();
                    if (gt >= GoldTotal) {
                        return false;
                    }
                }

                if (ExpTotal > 0)
                {
                    long et = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.ExpIncrea).Select(m => m.Value).Sum();
                    if (et >= ExpTotal)
                    {
                        return false;
                    }
                }

                if (LuckyTotal > 0)
                {
                    long lucky = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.Lucky).Select(m => m.Value).Sum();
                    if (lucky >= LuckyTotal)
                    {
                        return false;
                    }
                }

                if (DropRate > 0) {
                    long rateTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.BurstIncrea).Select(m => m.Value).Sum();
                    if (rateTotal >= DropRate)
                    {
                        return false;
                    }
                }

                if (DropQuality > 0)
                {
                    long qualityTotal = equip.AttrEntryList.Where(m => m.Key == (int)AttributeEnum.QualityIncrea).Select(m => m.Value).Sum();
                    if (qualityTotal >= DropQuality)
                    {
                        return false;
                    }
                }


                if ((EquipQuanlity.GetValueOrDefault(item.GetQuality(), false) || item.Level < EquipLevel || EquipRole.GetValueOrDefault(role, false))
                    && equip.Part <= 10 && !item.IsLock && equip.Quality < 5)
                {
                    return true;
                }
            }
            //else if (item.Type == ItemType.SkillBox)
            //{
            //    SkillBook skillBook = item as SkillBook;

            //    int role = skillBook.SkillConfig.Role;
            //    if (SkillBookRole.GetValueOrDefault(role, false) || item.Level < SkillBookLevel)
            //    {
            //        return true;
            //    }
            //}

            return false;
        }
    }
}
