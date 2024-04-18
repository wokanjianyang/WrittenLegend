using System;
using System.Collections.Generic;
using System.Linq;

namespace Game
{

    public partial class ArtifactConfigCategory
    {
        public ArtifactConfig GetByItemId(int itemId)
        {
            return this.list.Where(m => m.ItemId == itemId).FirstOrDefault();
        }

        public List<ArtifactConfig> GetListByType(ArtifactType type)
        {
            return this.list.Where(m => m.Type == (int)type).ToList();
        }
    }

    public enum ArtifactType
    {
        BossBattleRate = 1,
        BossTicketAd = 2,
        EquipTicketCd = 3,
        EquipTicketAd = 4,
        MineCount = 5,
        CardLimit = 6,
        ExclusiveLimit = 7,
        SkillLimit = 8,
        HolidomLimit = 9,
        SoulRingLimit = 10,
        StrengthLimit = 11,
        RefintLimit = 12,
        FashionLimit = 13,
        EquipStoneAd = 14,
        ExpGoldAd = 15,
        WingLimit = 16,
    }

}
